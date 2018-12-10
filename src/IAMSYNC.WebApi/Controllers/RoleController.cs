using Microsoft.AspNetCore.Mvc;

namespace IAMSYNC.WebApi.Controllers
{
    public class RoleController : ControllerBase
    {
        [HttpGet("/api/roles")]
        public IActionResult Index()
        {
            var amazonSecurityTokenServiceClient = new AmazonSecurityTokenServiceClient(_awsCredentials);

            var identityResponse =
                await amazonSecurityTokenServiceClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
            
            var accountArn = identityResponse.Account;

            return;
            View();
        }
    }
}