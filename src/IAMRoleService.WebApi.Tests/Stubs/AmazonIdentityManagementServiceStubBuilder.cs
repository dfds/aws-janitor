using Amazon.IdentityManagement.Model;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class AmazonIdentityManagementServiceStubBuilder
    {
        private readonly AmazonIdentityManagementServiceStub _amazonIdentityManagementServiceStub = new AmazonIdentityManagementServiceStub();
        public AmazonIdentityManagementServiceStub WithCreateRoleResponse(CreateRoleResponse createRoleResponse)
        {
            _amazonIdentityManagementServiceStub.CreateRoleResponse = createRoleResponse;

            return _amazonIdentityManagementServiceStub;
        }
    }
}