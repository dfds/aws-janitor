using System;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Domain.Events;

namespace AwsJanitor.WebApi.EventHandlers
{
    public class CapabilityCreatedEventHandler : IEventHandler<CapabilityCreatedDomainEvent>
    {
        
        public Task HandleAsync(CapabilityCreatedDomainEvent domainEvent)
        {
            Console.WriteLine($"Event received: CapabilityCreated with name: {domainEvent.Data.CapabilityName}");

           
            return Task.CompletedTask;
        }
    }
}