using System.Collections.Generic;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles
{
    public interface IPolicyTemplateRepository
    {
        Task<IEnumerable<PolicyTemplate>> GetLatestAsync();
    }
}