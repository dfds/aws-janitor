using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AwsJanitor.WebApi.Features.Roles
{
    [Route("api/policies")]
    public class PolicyController :ControllerBase
    {
        private readonly IAwsIdentityQueryClient _awsIdentityQueryClient;

        public PolicyController(IAwsIdentityQueryClient awsIdentityQueryClient)
        {
            _awsIdentityQueryClient = awsIdentityQueryClient;
        }

        [HttpGet("{capabilityName}")]
        public async Task<IActionResult> Get(string capabilityName)
        {
            var policies = await _awsIdentityQueryClient.GetPoliciesByCapabilityNameAsync(capabilityName);
            return Ok(policies);
        }
    }
}