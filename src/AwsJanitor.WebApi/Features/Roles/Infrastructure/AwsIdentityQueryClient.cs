using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Models;
using Newtonsoft.Json;

namespace AwsJanitor.WebApi.Features.Roles
{
    public class AwsIdentityQueryClient : IAwsIdentityQueryClient
    {
        private readonly IAmazonIdentityManagementService _client;

        public AwsIdentityQueryClient(IAmazonIdentityManagementService client)
        {
            _client = client;
        }

        public async Task<IEnumerable<PolicyDTO>> GetPoliciesByCapabilityNameAsync(string capabilityName)
        {
            var role = await GetRoleByCapabilityName(capabilityName);

            if (role == null) {throw new Exception($"No role could be found for capability: '{capabilityName}'");}
            
            var polices = await GetPolicies(role.RoleName);


            return polices;
        }

        
        public async Task<IEnumerable<PolicyDTO>> GetPolicies(string roleName)
        {
            var listRolePoliciesResponse =
                await _client.ListRolePoliciesAsync(new ListRolePoliciesRequest {RoleName = roleName});


            var policyTasks = listRolePoliciesResponse
                .PolicyNames
                .Select(async policyName =>
                    {
                        var getRolePolicyResponse = await _client.GetRolePolicyAsync(new GetRolePolicyRequest
                            {PolicyName = policyName, RoleName = roleName});

                        var document =  JsonConvert.DeserializeObject(Uri.UnescapeDataString(getRolePolicyResponse.PolicyDocument));
                        
                        var p = new PolicyDTO(
                            policyName: policyName,
                            policyDocument: document
                        );


                        return p;
                    }
                );

            var polices = await Task.WhenAll(policyTasks);

            return polices;
        }


        public async Task<Role> GetRoleByCapabilityName(string capabilityName)
        {
            var listRolesResponse = await _client.ListRolesAsync(new ListRolesRequest());

            var roles = listRolesResponse.Roles;
            var name = RoleName.Create(capabilityName);

            foreach (var role in roles)
            {
                var listRoleTagsResponse =
                    await _client.ListRoleTagsAsync(new ListRoleTagsRequest {RoleName = role.RoleName});
                if (listRoleTagsResponse.Tags.Any(t => t.Key == "capability" && t.Value == name) == false)
                {
                    continue;
                }

                role.Tags = listRoleTagsResponse.Tags;

                return role;
            }

            return null;
        }
    }
}