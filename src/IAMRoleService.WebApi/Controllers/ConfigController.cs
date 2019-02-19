using System.Text;
using System.Threading.Tasks;
using IAMRoleService.WebApi.Infrastructure.Aws;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api")]
    public class ConfigController : ControllerBase
    {
        public const string ContentType = "text/yaml";
        public const string KubeConfigFileName = "config";

        private readonly IParameterStore _parameterStore;

        public ConfigController(IParameterStore parameterStore)
        {
            _parameterStore = parameterStore;
        }

        [HttpGet("configs/kubeconfig")]
        [SwaggerOperation(Summary = "Download the default kubeconfig file")]
        [SwaggerResponse(200, "The default kubeconfig file")]
        public async Task<FileContentResult> DownloadDefaultKubeConfig()
        {
            var parameterValue = await _parameterStore.GetKubernetesConfigContent();

            return File(Encoding.UTF8.GetBytes(parameterValue), ContentType, KubeConfigFileName);
        }
    }
}