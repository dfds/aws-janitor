using System.Text;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api")]
    public class ConfigController : ControllerBase
    {
        public ConfigController()
        {
        }

        [HttpGet("configs/kubeconfig")]
        [SwaggerOperation(Summary = "Download the default kubeconfig file")]
        [SwaggerResponse(200, "The default kubeconfig file")]
        public FileContentResult DownloadDefaultKubeConfig()
        {
            var dummyYaml = "dummy: true";

            return File(Encoding.UTF8.GetBytes(dummyYaml), "text/yaml", "config");
        }
    }
}