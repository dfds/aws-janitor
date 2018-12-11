using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using IAMRoleService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly AwsAccountArn _accountArn;
        private readonly AmazonIdentityManagementServiceClient _client;

        public RoleController(AWSCredentials awsCredentials, RegionEndpoint regionEndpoint, AwsAccountArn accountArn)
        {
            _accountArn = accountArn;
            _client = new AmazonIdentityManagementServiceClient(awsCredentials, regionEndpoint);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateIAMRoleRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                return BadRequest("Name is invalid.");
            }

            var request = new CreateRoleRequest
            {
                RoleName = input.Name,
                AssumeRolePolicyDocument = @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""AWS"":""" + _accountArn + @"""},""Action"":""sts:AssumeRole"",""Condition"":{}}]}"
            };

            await _client.CreateRoleAsync(request);

            return Ok();
        }
    }
}