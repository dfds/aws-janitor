using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Models;
using AwsJanitor.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace AwsJanitor.WebApi.Controllers
{
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly ICreateIAMRoleRequestValidator _createIAMRoleRequestValidator;
        private readonly IAwsIdentityCommandClient _awsIdentityCommandClient;

        public RoleController(
            ICreateIAMRoleRequestValidator createIamRoleRequestValidator,
            IAwsIdentityCommandClient awsIdentityCommandClient
        )
        {
            _createIAMRoleRequestValidator = createIamRoleRequestValidator;
            _awsIdentityCommandClient = awsIdentityCommandClient;
        }

        [HttpGet("{rolename}/sync")]
        public async Task<IActionResult> SyncRole(string rolename)
        {
            var roleName = RoleName.Create(rolename);

            await _awsIdentityCommandClient.SyncRole(roleName);

            return Ok();
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

            var roleName = RoleName.Create(input.Name);
            var role = await _awsIdentityCommandClient.PutRoleAsync(roleName);

            return Ok(new
            {
                RoleArn = role.Arn
            });
        }
    }
}