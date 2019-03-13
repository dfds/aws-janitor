using System;
using Xunit;
namespace IAMRoleService.WebApi.Tests.Features.Roles.RoleName
{
    // Reads RoleName create will:
    public class CreateFacts
    {
        [Fact]
        public void Throw_Exception_If_Length_Of_RoleName_Is_Less_than_1()
        {
            // Arrange
            var subject = "";
            
            // Act / Assert
            Assert.Throws<ArgumentException>(() =>
            {
                IAMRoleService.WebApi.Features.Roles.Model.RoleName.Create(subject); });
        }
        
        
        [Fact]
        public void Throw_Exception_If_Length_Of_RoleName_Is_more_than_64()
        {
            // Arrange
            var subject = "jhdlaskhflheufhauinuaenclwunlawienliauwehhweiurhawilehranawiuniwn";
            
            // Act / Assert
            Assert.Throws<ArgumentException>(() =>
            {
                IAMRoleService.WebApi.Features.Roles.Model.RoleName.Create(subject); });
        }
        
              
        [Fact]
        public void Replaces_Spaces_With_Dash()
        {
            // Arrange
            var subject = "1 2 3 4";
            
            // Act 
            var roleName = IAMRoleService.WebApi.Features.Roles.Model.RoleName.Create(subject);
            
            // Assert
            var expectedRoleName = "1-2-3-4";
            Assert.Equal(expectedRoleName, roleName);
        }
        
        
        [Fact]
        public void Replaces_Uppercase_With_Lowercase()
        {
            // Arrange
            var subject = "ABCD";
            
            // Act 
            var roleName = IAMRoleService.WebApi.Features.Roles.Model.RoleName.Create(subject);
            
            // Assert
            var expectedRoleName = "abcd";
            Assert.Equal(expectedRoleName, roleName);
        }


        [Fact]
        public void Replace_Illigal_Chars_With_White_Spaces()
        {
            // not [\w+=,.@-]+
            
            // Arrange
            var subject = "#¤%/()";
            
            // Act
            var roleName = IAMRoleService.WebApi.Features.Roles.Model.RoleName.Create(subject);
            
            // Assert
            var expectedRoleName = "";
            Assert.Equal(expectedRoleName, roleName);
        }
    }
}