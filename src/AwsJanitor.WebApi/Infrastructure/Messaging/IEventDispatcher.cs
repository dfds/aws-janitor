using System.Threading.Tasks;
using AwsJanitor.WebApi.Domain.Events;

namespace AwsJanitor.WebApi.Infrastructure.Messaging
{
    public interface IEventDispatcher
    {
        Task Send(string generalDomainEventJson);
        Task SendAsync(GeneralDomainEvent generalDomainEvent);
    }
    
}