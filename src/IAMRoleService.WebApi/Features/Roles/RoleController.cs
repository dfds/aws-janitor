using System.Threading.Tasks;
using IAMRoleService.WebApi.Features.Roles;
using IAMRoleService.WebApi.Features.Roles.Model;
using IAMRoleService.WebApi.Infrastructure.Aws;
using IAMRoleService.WebApi.Models;
using IAMRoleService.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly ICreateIAMRoleRequestValidator _createIAMRoleRequestValidator;
        private readonly IAwsIdentityClient _awsIdentityClient;

        public RoleController(
            ICreateIAMRoleRequestValidator  createIamRoleRequestValidator, 
            IAwsIdentityClient awsIdentityClient
        )
        {
            _createIAMRoleRequestValidator = createIamRoleRequestValidator;
            _awsIdentityClient = awsIdentityClient;
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
            var role = await _awsIdentityClient.PutRoleAsync(roleName);
         
            return Ok(new
            {
                RoleArn = role.Arn
            });
        }
    }
}