using AwsJanitor.WebApi.Models;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Models
{
    public class KubernetesClusterNameFacts
    {
        [Fact]
        public void Can_Be_Compared_To_A_String()
        {
            // Arrange
            var kubernetesClusterName = new KubernetesClusterName("Henning");

            // Assert
            Assert.True("Henning".Equals(kubernetesClusterName));
        }
    }
}