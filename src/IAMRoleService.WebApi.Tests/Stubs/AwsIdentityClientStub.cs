using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using IAMRoleService.WebApi.Features.Roles;
using IAMRoleService.WebApi.Features.Roles.Model;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class AwsIdentityClientStub :IAwsIdentityClient
    {
        public Task<Role> PutRoleAsync(RoleName roleName)
        {
            return Task.FromResult(new Role {RoleName = RoleName.Create(roleName)});
        }

        public Task DeleteRoleAsync(RoleName roleName)
        {
            return Task.CompletedTask;
        }
    }
}