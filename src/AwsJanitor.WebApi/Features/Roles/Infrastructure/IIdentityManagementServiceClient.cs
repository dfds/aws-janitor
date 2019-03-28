using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles
{
    public interface IIdentityManagementServiceClient
    {
        Task DeleteRolePoliciesAsync(RoleName roleName, IEnumerable<string> namesOfPoliciesToDelete);
        Task DeleteRoleAsync(RoleName roleName);
        Task<IEnumerable<Policy>> PutPoliciesAsync(RoleName roleName, IEnumerable<Policy> policies);
    }
}