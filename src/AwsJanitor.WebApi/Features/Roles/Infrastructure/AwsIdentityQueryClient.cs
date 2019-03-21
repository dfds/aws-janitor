using System.Collections.Generic;
using System.Linq;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles
{
    public class AwsIdentityQueryClient :IAwsIdentityQueryClient
    {
        public IEnumerable<Policy> GetPoliciesByCapabilityName(string capabilityName)
        {
            return Enumerable.Empty<Policy>();
        }
    }
}