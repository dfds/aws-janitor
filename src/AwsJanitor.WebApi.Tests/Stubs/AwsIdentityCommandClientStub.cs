using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class AwsIdentityCommandClientStub : IAwsIdentityCommandClient
    {
        public IEnumerable<Role> Roles = new List<Role>();

        public Task<Role> PutRoleAsync(RoleName roleName)
        {
            return Task.FromResult(new Role {RoleName = RoleName.Create(roleName)});
        }

        public Task SyncRole(RoleName roleName)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Role>> GetRolesAsync()
        {
            return Task.FromResult(Roles);
        }

        public Task DeleteRoleAsync(RoleName roleName)
        {
            return Task.CompletedTask;
        }
    }
}