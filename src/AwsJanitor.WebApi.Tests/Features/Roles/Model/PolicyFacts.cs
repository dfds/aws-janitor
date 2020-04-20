using AwsJanitor.WebApi.Features.Roles.Model;
using System;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Features.Roles.Model
{
    public class PolicyFacts
    {
        [Fact]
        public void Properties_Will_Be_Set()
        {
            // Arrange
            var name = "foo";
            var document = "baaa";
            var policyTemplate = new PolicyTemplate(name, document);

            // Act
            var policy = Policy.Create(policyTemplate);

            // Assert
            Assert.Equal(name, policy.Name);
            Assert.Equal(document, policy.Document);
        }

        [Fact]
        public void CapabilityName_Will_Be_Replaced_In_Document()
        {
            // Arrange
            var name = "foo";
            var document = "foo capabilityName";
            var policyTemplate = new PolicyTemplate(name, document);
            var capabilityName = new CapabilityName("baaa");

            Func<PolicyTemplate, string> policyTemplateFormatter = (template) =>
            {
                return template.Document.Replace("capabilityName", capabilityName.ToString().ToLower());
            };

            // Act
            var policy = Policy.Create(policyTemplate, policyTemplateFormatter);
            
            // Assert
            var expectedDocument = "foo baaa";
            Assert.Equal(expectedDocument,policy.Document);
        }
    }
}