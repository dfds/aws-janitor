using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;

namespace AwsJanitor.WebApi.Features.Roles.Infrastructure
{
    public class AmazonIdentityManagementServiceWrapper :IAmazonIdentityManagementServiceWrapper
    {
        private readonly IAmazonIdentityManagementService _client;

        public AmazonIdentityManagementServiceWrapper(IAmazonIdentityManagementService client)
        {
            _client = client;
        }

        public Task<DeleteRolePolicyResponse> DeleteRolePolicyAsync(DeleteRolePolicyRequest deleteRolePolicyRequest)
        {
            return _client.DeleteRolePolicyAsync(deleteRolePolicyRequest);
        }

        public Task<ListRolePoliciesResponse> ListRolePoliciesAsync(ListRolePoliciesRequest listRolePoliciesRequest)
        {
            return _client.ListRolePoliciesAsync(listRolePoliciesRequest);
        }

        public Task<DeleteRoleResponse> DeleteRoleAsync(DeleteRoleRequest deleteRoleRequest)
        {
            return _client.DeleteRoleAsync(deleteRoleRequest);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}