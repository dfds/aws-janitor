using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAMRoleService.WebApi.Features.Roles;
using IAMRoleService.WebApi.Features.Roles.Model;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class PolicyRepositoryStub : IPolicyRepository
    {
        public Task<IEnumerable<Policy>> GetLatestAsync()
        {
            return Task.FromResult(Enumerable.Empty<Policy>());
        }
    }
}