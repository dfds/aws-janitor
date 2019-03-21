using System.Collections.Generic;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Models;

namespace AwsJanitor.WebApi.Features.Roles
{
    public interface IAwsIdentityQueryClient
    {
        Task<IEnumerable<PolicyDTO>> GetPoliciesByCapabilityNameAsync(string capabilityName);
    }
}