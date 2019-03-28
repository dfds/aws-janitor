using AwsJanitor.WebApi.Shared.Model;

namespace AwsJanitor.WebApi.Models
{
    public class KubernetesClusterName: StringSubstitutable
    {
        public KubernetesClusterName(string name) : base(name)
        {
        }
    }
}