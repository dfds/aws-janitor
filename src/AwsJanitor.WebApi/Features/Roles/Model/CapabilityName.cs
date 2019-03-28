using AwsJanitor.WebApi.Shared.Model;

namespace AwsJanitor.WebApi.Features.Roles.Model
{
    public class CapabilityName : StringSubstitutable
    {
        public CapabilityName(string capabilityName) : base(capabilityName)
        {
        }
    }
}