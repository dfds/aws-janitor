using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Tests.Stubs;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Features.Roles.infrastructure.AwsIdentityClient
{
    public class GetRolesFacts
    {
        [Fact]
        public async Task Only_Return_Role_With_Tag()
        {
            // Arrange
            const string MANAGED_BY = "managed-by";
            const string AWS_JANITOR = "AWS-Janitor";

            Role[] roles =
            {
                new Role
                {
                    RoleName = "good-role1", Tags = new List<Tag> {new Tag {Key = MANAGED_BY, Value = AWS_JANITOR}}
                },
                new Role
                {
                    RoleName = "good-role2", Tags = new List<Tag> {new Tag {Key = MANAGED_BY, Value = AWS_JANITOR}}
                },
                new Role {RoleName = "bad-role1"}
            };

         var amazonIdentityManagementServiceStubBuilder = new AmazonIdentityManagementServiceStubBuilder();
         
         var identityManagementServiceStub = amazonIdentityManagementServiceStubBuilder .WithRoles(roles);

         var client = new WebApi.Features.Roles.AwsIdentityClient(identityManagementServiceStub, null, null);
         
         // Act
         var managedRoles = await client.GetRolesAsync();
         
         // Assert
         Assert.DoesNotContain("bad-role1", managedRoles.Select(m => m.RoleName));
         Assert.Equal(2, managedRoles.Count());
        }
}

}