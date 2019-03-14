using AwsJanitor.WebApi.Models;

namespace AwsJanitor.WebApi.Validators
{
    public interface ICreateIAMRoleRequestValidator
    {
        bool TryValidateCreateRoleRequest(CreateIAMRoleRequest request, out string validationError);
    }
}