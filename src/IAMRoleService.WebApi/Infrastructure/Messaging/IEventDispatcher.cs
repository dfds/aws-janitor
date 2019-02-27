using System.Threading.Tasks;
using IAMRoleService.WebApi.Domain.Events;

namespace IAMRoleService.WebApi.Infrastructure.Messaging
{
    public interface IEventDispatcher
    {
        Task Send(string generalDomainEventJson);
        Task SendAsync(GeneralDomainEvent generalDomainEvent);
    }
    
}