using System.Threading.Tasks;
using AwsJanitor.WebApi.Infrastructure.Aws;

namespace AwsJanitor.WebApi.Tests.Stubs
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