using System.Threading.Tasks;
using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.SecurityToken;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Infrastructure;
using AwsJanitor.WebApi.Features.Roles.Model;
using AwsJanitor.WebApi.Tests.Spies;
using AwsJanitor.WebApi.Tests.Stubs;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Features.Roles.Infrastructure.IdentityManagementServiceClient
{
    public class PutPoliciesFacts
    {
        [Fact]
        public async Task Will_Put_Policy_If_No_Policy_Is_Found()
        {
            // Arrange
            var amazonIdentityManagementServiceWrapperStub = new AmazonIdentityManagementServiceWrapperSpy();
            var identityManagementServiceClient = new WebApi.Features.Roles.Infrastructure.Persistence.IdentityManagementServiceClient(amazonIdentityManagementServiceWrapperStub);

            amazonIdentityManagementServiceWrapperStub.GetPolicyAsyncResponse =
                () => throw new NoSuchEntityException("");
            var policies = new []{Policy.Create(new PolicyTemplate("",""), new CapabilityName(""))};
            
            
            // Act
            var addedPolices = await identityManagementServiceClient.PutPoliciesAsync(new WebApi.Features.Roles.Model.RoleName(""), policies);
            
            
            // Assert
            Assert.Equal(policies, addedPolices);
        }
        
        
        [Fact]
        public async Task Will_Skip_If_Policy_Document_Is_The_same()
        {
            // Arrange
            var amazonIdentityManagementServiceWrapperStub = new AmazonIdentityManagementServiceWrapperSpy();
            var identityManagementServiceClient = new WebApi.Features.Roles.Infrastructure.Persistence.IdentityManagementServiceClient(amazonIdentityManagementServiceWrapperStub);

            var policyDocument = "foo";

            amazonIdentityManagementServiceWrapperStub.GetPolicyAsyncResponse =
                () =>  new GetRolePolicyResponse {PolicyDocument = policyDocument};
            var policies = new []{Policy.Create(new PolicyTemplate("",policyDocument), new CapabilityName(""))};
            
            
            // Act
            var addedPolices = await identityManagementServiceClient.PutPoliciesAsync(new WebApi.Features.Roles.Model.RoleName(""), policies);
            
            
            // Assert
            Assert.Empty(amazonIdentityManagementServiceWrapperStub.PutRolePolicyRequests);
            Assert.Empty(addedPolices);

        }
    }
}