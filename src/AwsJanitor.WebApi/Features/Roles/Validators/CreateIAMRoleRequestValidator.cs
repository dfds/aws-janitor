using System.Text.RegularExpressions;
using AwsJanitor.WebApi.Models;

namespace AwsJanitor.WebApi.Validators
{
    public class CreateIAMRoleRequestValidator : ICreateIAMRoleRequestValidator
    {
        public bool TryValidateCreateRoleRequest(CreateIAMRoleRequest request, out string validationError)
        {
            validationError = string.Empty;

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                validationError = "Name is invalid.";
                return false;
            }

            // Role name 64 char max.
            if (request.Name.Length > 64)
            {
                validationError = "Name is invalid. A maximum of 64 characters is allowed.";
                return false;
            }

            // Only alphanumeric and '+=,.@-_' allowed.
            var allowedCharactersPattern = "^[a-zA-Z0-9!@#$&()\\-`.+,/\"]*$";
            var match = Regex.Match(request.Name, allowedCharactersPattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                validationError = "Name is invalid. Only alphanumeric and '+=,.@-_' is allowed.";
                return false;
            }

            return true;
        }
    }
}