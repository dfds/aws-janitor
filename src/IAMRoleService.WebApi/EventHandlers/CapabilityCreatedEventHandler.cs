using System;
using System.Threading.Tasks;
using IAMRoleService.WebApi.Domain.Events;

namespace IAMRoleService.WebApi.EventHandlers
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