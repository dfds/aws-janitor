using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using IAMRoleService.WebApi.Features.Roles.Model;

namespace IAMRoleService.WebApi.Features.Roles
{
    public interface IAwsIdentityClient
    {
        Task <Role>PutRoleAsync(RoleName roleName);
        Task DeleteRoleAsync(RoleName roleName);
    }
}