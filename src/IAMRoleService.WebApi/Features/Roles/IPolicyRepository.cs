using System.Collections.Generic;
using System.Threading.Tasks;
using IAMRoleService.WebApi.Features.Roles.Model;

namespace IAMRoleService.WebApi.Features.Roles
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<Policy>> GetLatestAsync();
    }
}