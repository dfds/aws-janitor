using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.SecurityToken;
using IAMRoleService.WebApi.Features.Roles;
using IAMRoleService.WebApi.Features.Roles.Model;
using Xunit;

namespace IAMRoleService.IntegrationTests.Features.Roles
{
    public class AwsIdentityClientFacts
    {
        [Fact]
        public async Task Can_Create_A_Role()
        {
            // Arrange
            var regionalEndpoint = RegionEndpoint.EUWest1;
            var amazonIdentityManagementServiceClient = new AmazonIdentityManagementServiceClient(regionalEndpoint);
            var amazonSecurityTokenServiceClient = new AmazonSecurityTokenServiceClient(regionalEndpoint);
            var fakePolicyRepository = new fakePolicyRepository();

            var awsIdentityClient = new AwsIdentityClient(
                amazonIdentityManagementServiceClient,
                amazonSecurityTokenServiceClient,
                fakePolicyRepository);

            var roleName = RoleName.Create("test-role-do-delete");
            
            try
            {
                // Act
                var role = await awsIdentityClient.EnsureRoleExistsAsync(roleName);

                
                // Assert
                
            }
            finally
            {
                await awsIdentityClient.DeleteRoleAsync(roleName);
            }
        }

        public class fakePolicyRepository : IPolicyRepository
        {
            public Task<IEnumerable<Policy>> GetLatestAsync()
            {
                return Task.FromResult(Enumerable.Empty<Policy>());
            }
        }
    }
}