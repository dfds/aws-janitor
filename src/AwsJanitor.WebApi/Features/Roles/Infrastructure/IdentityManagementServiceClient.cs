using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence
{
    public class IdentityManagementServiceClient :IIdentityManagementServiceClient
    {
        private readonly IAmazonIdentityManagementServiceWrapper _client;

        public IdentityManagementServiceClient(IAmazonIdentityManagementServiceWrapper client)
        {
            _client = client;
        }

        public async Task DeleteRolePoliciesAsync(RoleName roleName, IEnumerable<string> namesOfPoliciesToDelete)
        {
            foreach (var policyName in namesOfPoliciesToDelete)
            {
                await _client.DeleteRolePolicyAsync(new DeleteRolePolicyRequest
                {
                    RoleName = roleName,
                    PolicyName = policyName
                });
            }
        }
        
        
        public async Task DeleteRoleAsync(RoleName roleName)
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