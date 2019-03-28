using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;

namespace AwsJanitor.WebApi.Features.Roles.Infrastructure
{
    public interface IAmazonIdentityManagementServiceWrapper
    {
        Task<DeleteRolePolicyResponse> DeleteRolePolicyAsync(DeleteRolePolicyRequest deleteRolePolicyRequest);
        Task<ListRolePoliciesResponse> ListRolePoliciesAsync(ListRolePoliciesRequest listRolePoliciesRequest);
        Task<DeleteRoleResponse> DeleteRoleAsync(DeleteRoleRequest deleteRoleRequest);
        void Dispose();
    }
}