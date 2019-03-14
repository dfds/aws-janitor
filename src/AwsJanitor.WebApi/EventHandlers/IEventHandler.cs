using System;
using System.Threading.Tasks;

namespace AwsJanitor.WebApi.EventHandlers
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T domainEvent);
    }
}