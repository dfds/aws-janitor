using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.SecurityToken;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Infrastructure;
using AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Tests.Stubs;
using Xunit;

namespace AwsJanitor.IntegrationTests.Features.Roles
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
            var fakePolicyRepository = new FakePolicyTemplateRepository();
            var identityManagementClient = new IdentityManagementServiceClient(new AmazonIdentityManagementServiceWrapper(amazonIdentityManagementServiceClient));


            var awsIdentityClient = new AwsIdentityCommandClient(
                amazonIdentityManagementServiceClient,
                amazonSecurityTokenServiceClient,
                fakePolicyRepository, 
                identityManagementClient
            );

            var roleName = RoleName.Create("test-role-do-delete-33");
            var role = await awsIdentityClient.EnsureRoleExistsAsync(roleName);

            try
            {
                // Act

                
                // Assert
                
            }
            finally
            {
                await identityManagementClient.DeleteRoleAsync(roleName);
            }
        }

        public class FakePolicyTemplateRepository : IPolicyTemplateRepository
        {
            public Task<IEnumerable<PolicyTemplate>> GetLatestAsync()
            {
                return Task.FromResult(Enumerable.Empty<PolicyTemplate>());
            }
        }
    }
}