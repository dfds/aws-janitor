using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace IAMRoleService.WebApi.Infrastructure.Aws
{

    public class ParameterStore : IParameterStore
    {
        private readonly RegionEndpoint _region;

        public ParameterStore(RegionEndpoint region)
        {
            _region = region;
        }

        public async Task<string> GetKubernetesConfigContent()
        {
            using (IAmazonSimpleSystemsManagement client = new AmazonSimpleSystemsManagementClient(_region))
            {
                var request = new GetParameterRequest
                {
                    Name = "/eks/hellman/default_user",
                    WithDecryption = true
                };
                var response = await client.GetParameterAsync(request);

                return response.Parameter.Value;
            }
        }
    }
}