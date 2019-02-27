using System;
using System.Threading.Tasks;

namespace IAMRoleService.WebApi.EventHandlers
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T domainEvent);
    }
}