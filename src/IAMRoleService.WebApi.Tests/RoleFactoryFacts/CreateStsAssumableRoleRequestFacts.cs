using Xunit;

namespace IAMRoleService.WebApi.Tests.RoleFactoryFacts
{
    public class CreateStsAssumableRoleRequestFacts
    {
        [Fact]
        public void Will_Set_RoleName()
        {
            var accountArn = "foo";
            var roleName = "baa";
            var sut = new RoleFactory(null,null);


            // Act
            var assumableRoleRequest = sut.CreateStsAssumableRoleRequest(accountArn, roleName);


            // Assert
            Assert.Equal(roleName, assumableRoleRequest.RoleName);
        }


        [Fact]
        //allowed or denied access to a resource. The
        public void will_Allow_Access_for_Account()
        {
            var accountArn = "foo";
            var roleName = "baa";
            var sut = new RoleFactory(null,null);


            // Act
            var assumableRoleRequest = sut.CreateStsAssumableRoleRequest(accountArn, roleName);


            // Assert
            var expectedSubstring = @"{""Effect"":""Allow"",""Principal"":{""AWS"":""" + accountArn + @"""}";
            Assert.Contains(expectedSubstring, assumableRoleRequest.AssumeRolePolicyDocument);
        }
    }
}