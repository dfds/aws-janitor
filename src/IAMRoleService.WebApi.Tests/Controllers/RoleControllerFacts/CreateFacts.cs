using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Infrastructure.Aws;
using AwsJanitor.WebApi.Tests.Builders;
using AwsJanitor.WebApi.Tests.Stubs;
using AwsJanitor.WebApi.Validators;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Controllers.RoleControllerFacts
{
    // RoleController / Create Will..
    public class CreateFacts
    {
        [Fact]
        public async Task Return_400_If_Request_Is_Invalid()
        {
            using (var builder = new HttpClientBuilder())
            {
                // Arrange
                var client = builder
                    .WithService<ICreateIAMRoleRequestValidator>(new CreateIAMRoleRequestValidatorStub(false))
                    .WithService(RegionEndpoint.CNNorth1)
                    .Build();

                
                // Act
                var stubInput = "{\"name\":\"foo\"}";
                var response = await client.PostAsync(
                    "api/roles", 
                    new JsonContent(stubInput)
                );
                
                
                // Assert
                Assert.Equal(
                    HttpStatusCode.BadRequest, 
                    response.StatusCode
                );
            }
        }
        
        [Fact]
        public async Task Return_200_If_Request_Is_Valid()
        {
            using (var builder = new HttpClientBuilder())
            {
                // Arrange
                var client = builder
                    .WithService<ICreateIAMRoleRequestValidator>(new CreateIAMRoleRequestValidatorStub(true))
                    .WithService<IAwsIdentityClient>(new AwsIdentityClientStub())
                    .WithService(RegionEndpoint.CNNorth1)
                    .Build();

                
                // Act
                var stubInput = "{\"name\":\"foo\"}";
                var response = await client.PostAsync(
                    "api/roles", 
                    new JsonContent(stubInput)
                );
                
                
                // Assert
                Assert.Equal(
                    HttpStatusCode.OK, 
                    response.StatusCode
                );
            }
        }

    }
}