using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using IAMRoleService.WebApi.Models;
using IAMRoleService.WebApi.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly AmazonIdentityManagementServiceClient _client;
        private readonly AmazonSecurityTokenServiceClient _securityTokenServiceClient;
        private readonly ICreateIAMRoleRequestValidator _createIAMRoleRequestValidator;

        public RoleController(ICreateIAMRoleRequestValidator createIAMRoleRequestValidator, AmazonIdentityManagementServiceClient identityManagementServiceClient, 
            AmazonSecurityTokenServiceClient securityTokenServiceClient)
        {
            _client = identityManagementServiceClient;
            _securityTokenServiceClient = securityTokenServiceClient;
            _createIAMRoleRequestValidator = createIAMRoleRequestValidator;
        }

        [HttpPost("")]
        [SwaggerOperation(
            Summary = "Creates a role",
            Description = "Creates an IAM role in AWS."
        )]
        [SwaggerResponse(200, "The IAM role was created.")]
        [SwaggerResponse(400, "The role name is invalid.")]
        [SwaggerResponse(500, "An error occured trying to create the IAM role.")]
        public async Task<IActionResult> Create([FromBody, SwaggerParameter("Create role request.", Required = true)]
                                                CreateIAMRoleRequest input)
        {
            if (!_createIAMRoleRequestValidator.TryValidateCreateRoleRequest(input, out string validationError))
            {
                Log.Warning($"Create role called with invalid input. Validation error: {validationError}");
                return BadRequest(validationError);
            }

            var request = await CreateRoleRequest(input.Name);
            var response = await _client.CreateRoleAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                var metadata = string.Join(", ", response.ResponseMetadata.Metadata.Select(m => $"{m.Key}:{m.Value}"));
                Log.Error($"Error creating role. Status code was {response.HttpStatusCode}, metadata: {metadata}");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured trying to create the IAM role.");
            }

            return Ok(new
            {
                RoleArn = response.Role.Arn
            });
        }

        private async Task<CreateRoleRequest> CreateRoleRequest(string roleName)
        {
            var identityResponse = await _securityTokenServiceClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
            var accountArn = new AwsAccountArn(identityResponse.Account);

            return new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument = @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""AWS"":""" + accountArn + @"""},""Action"":""sts:AssumeRole"",""Condition"":{}}]}"
            };
        }
    }
}