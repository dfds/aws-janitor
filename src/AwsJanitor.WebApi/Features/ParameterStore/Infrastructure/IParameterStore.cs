using System.Threading.Tasks;

namespace AwsJanitor.WebApi.Infrastructure.Aws
{
    public interface IParameterStore
    {
        Task<string> GetKubernetesConfigContent();
    }
}