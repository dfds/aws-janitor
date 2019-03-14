using System;
using AwsJanitor.WebApi.Models;
using AwsJanitor.WebApi.Validators;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class CreateIAMRoleRequestValidatorStub : ICreateIAMRoleRequestValidator
    {
        private readonly bool _positiveResult;

        public CreateIAMRoleRequestValidatorStub(bool positiveResult)
        {
            _positiveResult = positiveResult;
        }

        public bool TryValidateCreateRoleRequest(CreateIAMRoleRequest request, out string validationError)
        {
            if (_positiveResult)
            {
                validationError = String.Empty;
                return true;
            }
            validationError = "It is wrong";
            return false;
        }
    }
}