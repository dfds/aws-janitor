using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;

namespace IAMRoleService.WebApi
{
    public interface IRoleFactory
    {
        Task<Role> CreateStsAssumableRoleAsync(string roleName);
    }
}