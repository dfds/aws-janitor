using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using Amazon.SecurityToken.Model;
using IAMRoleService.WebApi.Tests.Stubs;
using Xunit;

namespace IAMRoleService.WebApi.Tests.RoleFactoryFacts
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

            var sut = new RoleFactory(
                amazonIdentityManagementServiceStubBuilder.WithCreateRoleResponse(createRoleResponse),
                amazonSecurityTokenServiceStubBuilder.WithGetCallerIdentityResponse(getCallerIdentityResponse)
            );

            // Act / Assert
            await Assert.ThrowsAsync<Exception>(() => sut.CreateStsAssumableRoleAsync(roleName));
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

            var sut = new RoleFactory(
                amazonIdentityManagementServiceStubBuilder.WithCreateRoleResponse(createRoleResponse),
                amazonSecurityTokenServiceStubBuilder.WithGetCallerIdentityResponse(getCallerIdentityResponse)
            );


            // Act 
            var resultRole = await sut.CreateStsAssumableRoleAsync(roleName);


            // Assert
            Assert.Same(role, resultRole);
        }
    }
}