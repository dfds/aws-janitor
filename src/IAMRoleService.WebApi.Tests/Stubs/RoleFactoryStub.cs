using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class RoleFactoryStub : IRoleFactory
    {
        public Task<Role> CreateStsAssumableRoleAsync(string roleName)
        {
            var role = new Role();
            role.RoleName = roleName;
            role.Arn = "notAValidArn";

            
            return Task.FromResult(role);
        }
    }
}