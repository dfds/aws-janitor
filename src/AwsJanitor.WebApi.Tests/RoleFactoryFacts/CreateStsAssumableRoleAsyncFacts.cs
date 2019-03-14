using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Amazon.SecurityToken.Model;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Infrastructure.Aws;
using AwsJanitor.WebApi.Tests.Stubs;
using Xunit;

namespace AwsJanitor.WebApi.Tests.RoleFactoryFacts
{
    public class CreateStsAssumableRoleAsyncFacts
    {
        [Fact]
        public async Task Will_Throw_A_Exception_If_CreateRoleAsync_Response_Is_Not_Ok()
        {
            // Arrange
            var createRoleResponse = new CreateRoleResponse();
            createRoleResponse.HttpStatusCode = HttpStatusCode.ServiceUnavailable;
            createRoleResponse.ResponseMetadata = new ResponseMetadata();
            createRoleResponse.ResponseMetadata.Metadata["foo"] = "bar";

            var amazonIdentityManagementServiceStubBuilder = new AmazonIdentityManagementServiceStubBuilder();

            var getCallerIdentityResponse = new GetCallerIdentityResponse();
            getCallerIdentityResponse.Account = "AccountDoesNotMatter";
            var amazonSecurityTokenServiceStubBuilder = new AmazonSecurityTokenServiceStubBuilder();
            var roleName = "doesNotMatter";

            var sut = new AwsIdentityClient(
                amazonIdentityManagementServiceStubBuilder.WithCreateRoleResponse(createRoleResponse),
                amazonSecurityTokenServiceStubBuilder.WithGetCallerIdentityResponse(getCallerIdentityResponse),
                new PolicyRepositoryStub()
            );

            // Act / Assert
            await Assert.ThrowsAsync<Exception>(() => sut.PutRoleAsync(new RoleName(roleName)));
        }


        [Fact]
        public async Task Will_Return_A_Role_If_CreateRoleAsync_Response_Is_Ok()
        {
            // Arrange
            var role = new Role();
            var createRoleResponse = new CreateRoleResponse();
            createRoleResponse.HttpStatusCode = HttpStatusCode.OK;
            createRoleResponse.Role = role;
            var amazonIdentityManagementServiceStubBuilder = new AmazonIdentityManagementServiceStubBuilder();


            var getCallerIdentityResponse = new GetCallerIdentityResponse();
            getCallerIdentityResponse.Account = "AccountDoesNotMatter";
            var amazonSecurityTokenServiceStubBuilder = new AmazonSecurityTokenServiceStubBuilder();
            var roleName = "doesNotMatter";

            var sut = new AwsIdentityClient(
                amazonIdentityManagementServiceStubBuilder.WithCreateRoleResponse(createRoleResponse),
                amazonSecurityTokenServiceStubBuilder.WithGetCallerIdentityResponse(getCallerIdentityResponse),
                new PolicyRepositoryStub()
            );


            // Act 
            var resultRole = await sut.PutRoleAsync(new RoleName(roleName));


            // Assert
            Assert.Same(role, resultRole);
        }
    }
}