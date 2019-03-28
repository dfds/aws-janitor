using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Models;

namespace AwsJanitor.WebApi.Features.Roles
{
    public class AwsIdentityCommandClient : IDisposable, IAwsIdentityCommandClient
    {
        private const  string MANAGED_BY = "managed-by";
        private const  string AWS_JANITOR = "AWS-Janitor";

        private readonly IAmazonIdentityManagementService _client;
        private readonly IIdentityManagementServiceClient _identityManagementClient;
        private readonly IAmazonSecurityTokenService _securityTokenServiceClient;
        private readonly IPolicyTemplateRepository _policyTemplateRepository;

        public AwsIdentityCommandClient(
            IAmazonIdentityManagementService client,
            IAmazonSecurityTokenService securityTokenServiceClient, 
            IPolicyTemplateRepository policyTemplateRepository, 
            IIdentityManagementServiceClient identityManagementClient)
        {
            _client = client;
            _securityTokenServiceClient = securityTokenServiceClient;
            _policyTemplateRepository = policyTemplateRepository;
            _identityManagementClient = identityManagementClient;
        }

        public async Task SyncRole(RoleName roleName)
        {
            await SyncTags(roleName);
            await SyncPoliciesAsync(roleName);
        }
        
        
        
        public async Task SyncTags(RoleName roleName)
        {
           await _client.TagRoleAsync(new TagRoleRequest
            {
                RoleName = roleName,
                Tags = new List<Tag>
                {
                    new Tag {Key = MANAGED_BY, Value = AWS_JANITOR},
                    new Tag {Key = "capability", Value = roleName},
                    new Tag {Key = "last updated", Value = DateTime.UtcNow.ToString("u")}
                }
            });
        }

        public async Task<Role> PutRoleAsync(RoleName roleName)
        {
            var role = await EnsureRoleExistsAsync(roleName);

            await SyncPoliciesAsync(roleName);

            
            return role;
        }

        public async Task<Role> EnsureRoleExistsAsync(RoleName roleName)
        {
            var identityResponse =
                await _securityTokenServiceClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
            var accountArn = new AwsAccountArn(identityResponse.Account);

            var request = CreateRoleRequest(accountArn, roleName);

            try
            {
                var response = await _client.CreateRoleAsync(request);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    var metadata = string.Join(", ",
                        response.ResponseMetadata.Metadata.Select(m => $"{m.Key}:{m.Value}"));
                    throw new Exception(
                        $"Error creating role: \"{roleName}\". Status code was {response.HttpStatusCode}, metadata: {metadata}");
                }

                return response.Role;
            }
            catch (EntityAlreadyExistsException)
            {
                // Role exists we are happy
                var getRoleRequest = new GetRoleRequest {RoleName = roleName};
                var getRoleResponse = await _client.GetRoleAsync(getRoleRequest);

                return getRoleResponse.Role;
            }
        }


        public CreateRoleRequest CreateRoleRequest(AwsAccountArn accountArn, RoleName roleName)
        {
            return new CreateRoleRequest
            {
                RoleName = roleName,
                Tags = new List<Tag>
                {
                    new Tag{Key = MANAGED_BY,Value = AWS_JANITOR},
                    new Tag{Key = "capability",Value = roleName}
                },
                Description = $"sts assumable role for capability: '{roleName}'. Managed by {AWS_JANITOR}",
                AssumeRolePolicyDocument =
                    @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""Federated"":""" +
                    accountArn + ":saml-provider/ADFS" +
                    @"""},""Action"":""sts:AssumeRoleWithSAML"", ""Condition"": {""StringEquals"": {""SAML:aud"": ""https://signin.aws.amazon.com/saml""}}}]}"
            };
        }
        

        /// <returns>Bool true if any change to policies have been made</returns>
        public async Task<bool> SyncPoliciesAsync(RoleName roleName)
        {
            var policyTemplates = await _policyTemplateRepository.GetLatestAsync();

            var policiesResponse =
                await _client.ListRolePoliciesAsync(new ListRolePoliciesRequest {RoleName = roleName});


            var namesOfPoliciesToDelete =
                FindPolicyNamesWithoutTemplates(policiesResponse.PolicyNames, policyTemplates);

            await _identityManagementClient.DeleteRolePoliciesAsync(roleName, namesOfPoliciesToDelete);

            var capabilityName = new CapabilityName(roleName);
            var policies = policyTemplates.Select(p => Policy.Create(p, capabilityName));
            var policiesAdded = await _identityManagementClient.PutPoliciesAsync(roleName, policies);

            return namesOfPoliciesToDelete.Any() & policiesAdded.Any();
        }

        public  IEnumerable<string> FindPolicyNamesWithoutTemplates(
            IEnumerable<string> policyNames, 
            IEnumerable<PolicyTemplate> policyTemplates
        )
        {
            var policyTemplateNames = policyTemplates.Select(pt =>
                pt.Name
            );
            
            var namesOfPoliciesToDelete = 
                policyNames.Where(p => policyTemplateNames.Contains(p) == false);

            
            return namesOfPoliciesToDelete;
        }
    

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}