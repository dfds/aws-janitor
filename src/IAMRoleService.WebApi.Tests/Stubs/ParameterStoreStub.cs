using System.Threading.Tasks;
using IAMRoleService.WebApi.Infrastructure.Aws;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class ParameterStoreStub : IParameterStore
    {
        private readonly string _content;

        public ParameterStoreStub(string content)
        {
            _content = content;
        }

        public Task<string> GetKubernetesConfigContent()
        {
            return Task.FromResult(_content);
        }
    }
}