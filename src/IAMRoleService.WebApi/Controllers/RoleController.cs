using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using IAMRoleService.WebApi.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
// using IAMRoleService.WebApi.Controllers.Models;

namespace IAMRoleService.WebApi.Controllers
{
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private BasicAWSCredentials _awsCredentials;
        private readonly AmazonIdentityManagementServiceClient _client;

        public RoleController(BasicAWSCredentials awsCredentials)
        {
            _awsCredentials = awsCredentials;
            _client = new AmazonIdentityManagementServiceClient(
                _awsCredentials,
                RegionEndpoint.EUCentral1
            );
        }


        [HttpGet("")]
        public async Task<IEnumerable<string>> Index()
        {
            var listRolesRequest = new ListRolesRequest();

            var response = await _client.ListRolesAsync(listRolesRequest);


            return response.Roles.Select(r => r.RoleName);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateIAMRoleRequest createIAMRoleRequest)
        {
            if(string.IsNullOrEmpty(createIAMRoleRequest?.RoleName)) {throw  new ArgumentException("name must not be null");}

            var accountArn = "arn:aws:iam::AccountNumber-WithoutHyphens:root";
            var createRoleRequest = new CreateRoleRequest 
            {
                RoleName = createIAMRoleRequest.RoleName,
                AssumeRolePolicyDocument =
                    @"{""Version"":""2012-10-17"",""Statement"":[{""Effect"":""Allow"",""Principal"":{""AWS"":"""+ accountArn + @"""},""Action"":""sts:AssumeRole"",""Condition"":{}}]}"

            };

            var createRoleResponse = await _client.CreateRoleAsync(createRoleRequest);

            return Ok(new {arn = createRoleResponse.Role.Arn});
        }
    }
}