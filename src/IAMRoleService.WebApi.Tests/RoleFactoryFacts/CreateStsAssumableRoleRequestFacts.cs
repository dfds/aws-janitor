using IAMRoleService.WebApi.Features.Roles;
using IAMRoleService.WebApi.Features.Roles.Model;
using IAMRoleService.WebApi.Infrastructure.Aws;
using IAMRoleService.WebApi.Models;
using Xunit;

namespace IAMRoleService.WebApi.Tests.RoleFactoryFacts
{
    public class CreateStsAssumableRoleRequestFacts
    {
        [Fact]
        public void Will_Set_RoleName()
        {
            var accountArn = new AwsAccountArn("foo");
            var roleName = new RoleName("baa");
            var sut = new AwsIdentityClient(null,null, null);


            // Act
            var assumableRoleRequest = sut.CreateRoleRequest(accountArn, roleName);


            // Assert
            Assert.Equal(roleName, assumableRoleRequest.RoleName);
        }

        [Fact]
        //allowed or denied access to a resource. The
        public void Principal_Will_Point_To_Federated_Login()
        {
            var accountArn = new AwsAccountArn("foo");
            var roleName = new RoleName("baa");
            var sut = new AwsIdentityClient(null,null, null);


            // Act
            var assumableRoleRequest = sut.CreateRoleRequest(accountArn, roleName);


            // Assert
            var expectedSubstring = "Principal\":{\"Federated\":\"arn:aws:iam::foo:saml-provider/ADFS\"}";//@"{""Effect"":""Allow"",""Principal"":{""AWS"":""" + accountArn + @"""}";
            Assert.Contains(expectedSubstring, assumableRoleRequest.AssumeRolePolicyDocument);
        }
    }
}