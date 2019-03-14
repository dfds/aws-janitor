using System;
using Xunit;
using AwsJanitor.WebApi.Models;
using AwsJanitor.WebApi.Validators;

namespace AwsJanitor.WebApi.Tests
{
    public class CreateIAMRoleRequestValidatorTest
    {
        [Fact]
        public void TryValidateCreateRoleRequest_GivenValidInput_Validates()
        {
            // Arrange
            var sut = new CreateIAMRoleRequestValidator();
            var request = new CreateIAMRoleRequest
            {
                Name = "MyRole"
            };
            var validationErros = string.Empty;

            // Act
            var validRequest = sut.TryValidateCreateRoleRequest(request, out validationErros);

            // Assert
            Assert.True(validRequest);
            Assert.Equal(string.Empty, validationErros);
        }

        [Fact]
        public void TryValidateCreateRoleRequest_GivenInvalidCharacter_DoesNotValidate()
        {
            // Arrange
            var sut = new CreateIAMRoleRequestValidator();
            var request = new CreateIAMRoleRequest
            {
                Name = "å"
            };
            var validationErros = string.Empty;

            // Act
            var validRequest = sut.TryValidateCreateRoleRequest(request, out validationErros);

            // Assert
            Assert.False(validRequest);
            Assert.NotEqual(string.Empty, validationErros);
        }

        [Fact]
        public void TryValidateCreateRoleRequest_GivenTooLongRoleName_DoesNotValidate()
        {
            // Arrange
            var sut = new CreateIAMRoleRequestValidator();
            var roleNameLongerThan64Characters = new string('*', 65);
            var request = new CreateIAMRoleRequest
            {
                Name = roleNameLongerThan64Characters
            };
            var validationErros = string.Empty;

            // Act
            var validRequest = sut.TryValidateCreateRoleRequest(request, out validationErros);

            // Assert
            Assert.False(validRequest);
            Assert.NotEqual(string.Empty, validationErros);
        }
    }
}
