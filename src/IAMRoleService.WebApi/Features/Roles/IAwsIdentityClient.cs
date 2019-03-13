using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;

namespace IAMRoleService.WebApi.Features.Roles
{
    public interface IAwsIdentityClient
    {
        Task <Role>PutRoleAsync(string roleName);
        Task DeleteRoleAsync(string roleName);
    }
}