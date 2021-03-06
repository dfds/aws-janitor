using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using AwsJanitor.WebApi.Models;

namespace AwsJanitor.WebApi.Infrastructure.Aws
{

    public class ParameterStore : IParameterStore
    {
        private readonly RegionEndpoint _region;
        private readonly KubernetesClusterName _kubernetesClusterName;

        public ParameterStore(RegionEndpoint region, KubernetesClusterName kubernetesClusterName)
        {
            _region = region;
            _kubernetesClusterName = kubernetesClusterName;
        }

        public async Task<string> GetKubernetesConfigContent()
        {
            using (IAmazonSimpleSystemsManagement client = new AmazonSimpleSystemsManagementClient(_region))
            {
                var name = $"/eks/{_kubernetesClusterName}/default_user";
                var request = new GetParameterRequest
                {
                    Name = name,
                    WithDecryption = true
                };
                var response = await client.GetParameterAsync(request);

                return response.Parameter.Value;
            }
        }
    }
}