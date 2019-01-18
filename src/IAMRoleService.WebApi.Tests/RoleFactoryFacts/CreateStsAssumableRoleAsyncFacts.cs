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

            Assert.ThrowsAsync<Exception>(() => sut.CreateStsAssumableRoleAsync(roleName));
            // Assert
        }
    }
}