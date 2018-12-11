using Microsoft.AspNetCore.Mvc;

namespace IAMRoleService.WebApi.Controllers
{
    [ApiController]
    public class HelloController : ControllerBase {
        [HttpGet("")]
        public ActionResult<string> Get() {
            return "Hello World";
        }
    }
}
