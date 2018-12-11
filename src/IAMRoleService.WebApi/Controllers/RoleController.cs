using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
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
        public async Task<IActionResult> Create([FromBody] me input)
        {
            if (string.IsNullOrWhiteSpace(input.name))
            {
                return BadRequest("Name is invalid.");
            }

            var request = new CreateRoleRequest
            {
                RoleName = input.name,
                AssumeRolePolicyDocument = @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""AWS"":""" + _accountArn + @"""},""Action"":""sts:AssumeRole"",""Condition"":{}}]}"
            };

            var response = await _client.CreateRoleAsync(request);

            return Ok(new
            {
                arn = response.Role.Arn
            });
        }

        public class me
        {
            public string name { get; set; }
        }
    }

    public class AwsAccountArn
    {
        public AwsAccountArn(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }

        public override string ToString()
        {
            return $"arn:aws:iam::{AccountNumber}:root";
        }
    }
}