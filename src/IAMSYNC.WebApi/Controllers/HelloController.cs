using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IAMSYNC.WebApi.Controllers
{
    [ApiController]
    public class HelloController : ControllerBase {
        [HttpGet("")]
        public ActionResult<string> Get() {
            return "Hello World";
        }
    }
}
