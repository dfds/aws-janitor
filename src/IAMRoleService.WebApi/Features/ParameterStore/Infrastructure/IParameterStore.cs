using System.Threading.Tasks;

namespace IAMRoleService.WebApi.Infrastructure.Aws
{
    public interface IParameterStore
    {
        Task<string> GetKubernetesConfigContent();
    }
}