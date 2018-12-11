using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using IAMRoleService.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            if (TryValidateCreateRoleRequest(input, out string validationError))
            {
                Log.Warning($"Create role called with invalid input. Validation error: {validationError}");
                return BadRequest(validationError);
            }

            var request = new CreateRoleRequest
            {
                RoleName = input.Name,
                AssumeRolePolicyDocument = @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""AWS"":""" + _accountArn + @"""},""Action"":""sts:AssumeRole"",""Condition"":{}}]}"
            };

            var response = await _client.CreateRoleAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                var metadata = string.Join(", ", response.ResponseMetadata.Metadata.Select(m => $"{m.Key}:{m.Value}"));
                Log.Error($"Error creating role. Status code was {response.HttpStatusCode}, metadata: {metadata}");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured trying to create the IAM role.");
            }

            return Ok();
        }

        private bool TryValidateCreateRoleRequest(CreateIAMRoleRequest request, out string validationError)
        {
            validationError = string.Empty;

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                validationError = "Name is invalid.";
                return false;
            }

            // Role name 64 char max.
            if (request.Name.Length > 64)
            {
                validationError = "Name is invalid. A maximum of 64 characters is allowed.";
                return false;
            }

            // Only alphanumeric and '+=,.@-_' allowed.
            var allowedCharactersPattern = "^[a-zA-Z0-9!@#$&()\\-`.+,/\"]*$";
            var match = Regex.Match(request.Name, allowedCharactersPattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                validationError = "Name is invalid. Only alphanumeric and '+=,.@-_' is allowed.";
                return false;
            }

            return true;
        }
    }
}