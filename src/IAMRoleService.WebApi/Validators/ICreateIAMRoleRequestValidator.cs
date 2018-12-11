using IAMRoleService.WebApi.Models;

namespace IAMRoleService.WebApi.Validators
{
    public interface ICreateIAMRoleRequestValidator
    {
        bool TryValidateCreateRoleRequest(CreateIAMRoleRequest request, out string validationError);
    }
}