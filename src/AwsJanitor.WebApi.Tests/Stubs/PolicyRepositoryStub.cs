using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class PolicyRepositoryStub : IPolicyRepository
    {
        public Task<IEnumerable<Policy>> GetLatestAsync()
        {
            return Task.FromResult(Enumerable.Empty<Policy>());
        }
    }
}