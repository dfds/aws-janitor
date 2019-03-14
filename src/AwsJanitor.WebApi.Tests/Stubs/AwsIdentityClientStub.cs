using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
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