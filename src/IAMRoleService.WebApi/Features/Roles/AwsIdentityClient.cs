using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using IAMRoleService.WebApi.Models;

namespace IAMRoleService.WebApi.Features.Roles
{
    public class AwsIdentityClient : IDisposable, IAwsIdentityClient
    {
        private readonly IAmazonIdentityManagementService _client;
        private readonly IAmazonSecurityTokenService _securityTokenServiceClient;
        private readonly IPolicyRepository _policyRepository;

        public AwsIdentityClient(
            IAmazonIdentityManagementService client,
            IAmazonSecurityTokenService securityTokenServiceClient, 
            IPolicyRepository policyRepository
        )
        {
            _client = client;
            _securityTokenServiceClient = securityTokenServiceClient;
            _policyRepository = policyRepository;
        }


        public async Task<Role> PutRoleAsync(string roleName)
        {
            var role = await EnsureRoleExistsAsync(roleName);

            await PutRolePoliciesAsync(roleName);

            
            return role;
        }

        public async Task<Role> EnsureRoleExistsAsync(string roleName)
        {
            var identityResponse =
                await _securityTokenServiceClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
            var accountArn = new AwsAccountArn(identityResponse.Account);

            var request = CreateStsAssumableRoleRequest(accountArn, roleName);

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


        public CreateRoleRequest CreateStsAssumableRoleRequest(AwsAccountArn accountArn, string roleName)
        {
            return new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument =
                    @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""Federated"":""" +
                    accountArn + ":saml-provider/ADFS" +
                    @"""},""Action"":""sts:AssumeRoleWithSAML"", ""Condition"": {""StringEquals"": {""SAML:aud"": ""https://signin.aws.amazon.com/saml""}}}]}"
            };
        }

        private async Task PutRolePoliciesAsync(string roleName)
        {
            var policies = await _policyRepository.GetLatestAsync();
            var tasks = new List<Task>();
            foreach (var policy in policies)
            {
                tasks.Add(Task.Run(async () =>
                    {
                        var policyDocumentWithRoleName = policy.Document.Replace("teamName", roleName.ToLower());
                        var rolePolicyRequest = new PutRolePolicyRequest
                        {
                            RoleName = roleName,
                            PolicyName = policy.Name,
                            PolicyDocument = policyDocumentWithRoleName
                        };
                        await _client.PutRolePolicyAsync(rolePolicyRequest);
                    })
                );
            }

            Task.WaitAll(tasks.ToArray());
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var policiesResponse =
                await _client.ListRolePoliciesAsync(new ListRolePoliciesRequest {RoleName = roleName});
            foreach (var policyName in policiesResponse.PolicyNames)
            {
                await _client.DeleteRolePolicyAsync(new DeleteRolePolicyRequest
                {
                    RoleName = roleName,
                    PolicyName = policyName
                });
            }

            await _client.DeleteRoleAsync(new DeleteRoleRequest {RoleName = roleName});
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}