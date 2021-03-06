using System.Net;
using System.Threading.Tasks;
using Amazon.SecurityToken;
using AwsJanitor.WebApi.Controllers;
using AwsJanitor.WebApi.Infrastructure.Aws;
using AwsJanitor.WebApi.Tests.Builders;
using AwsJanitor.WebApi.Tests.Stubs;
using Xunit;

namespace AwsJanitor.WebApi.Tests.Controllers
{
    public class ConfigControllerFacts
    {
        [Fact]
        public void Has_correct_content_type()
        {
            Assert.Equal("text/yaml", ConfigController.ContentType);
        }

        [Fact]
        public void Has_correct_download_filename()
        {
            Assert.Equal("config", ConfigController.KubeConfigFileName);
        }

        [Fact]
        public async Task Can_download_file()
        {
            using (var builder = new HttpClientBuilder())
            {
                const string dummyContent = "foo";

                var client = builder
                    .WithService<IParameterStore>(new ParameterStoreStub(dummyContent))
                    .WithService<IAmazonSecurityTokenService>(new AmazonSecurityTokenServiceStub())
                    .Build();

                var response = await client.GetAsync("api/configs/kubeconfig");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(ConfigController.ContentType, response.Content.Headers.ContentType.MediaType);
                Assert.Equal("attachment", response.Content.Headers.ContentDisposition.DispositionType);
                Assert.Equal(ConfigController.KubeConfigFileName, response.Content.Headers.ContentDisposition.FileName);
                Assert.Equal(dummyContent, await response.Content.ReadAsStringAsync());
            }
        }
    }
}