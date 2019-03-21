using System.Collections.Generic;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles
{
    public interface IAwsIdentityQueryClient
    {
        IEnumerable<Policy> GetPoliciesByCapabilityName(string capabilityName);
    }
}