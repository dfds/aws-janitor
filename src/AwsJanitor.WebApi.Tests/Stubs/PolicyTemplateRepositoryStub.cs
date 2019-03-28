using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class PolicyTemplateRepositoryStub : IPolicyTemplateRepository
    {
        public Task<IEnumerable<PolicyTemplate>> GetLatestAsync()
        {
            return Task.FromResult(Enumerable.Empty<PolicyTemplate>());
        }
    }
}