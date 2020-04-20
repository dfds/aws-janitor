using System;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Domain.Events;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.EventHandlers
{
    public class ContextAddedToCapabilityEventHandler : IEventHandler<ContextAddedToCapabilityDomainEvent>
    {
        private readonly IAwsIdentityCommandClient _awsIdentityCommandClient;

        public ContextAddedToCapabilityEventHandler(IAwsIdentityCommandClient awsIdentityCommandClient) 
        {
            _awsIdentityCommandClient = awsIdentityCommandClient;
        }

        public Task HandleAsync(ContextAddedToCapabilityDomainEvent domainEvent)
        {
            var roleName = new RoleName(domainEvent.Data.CapabilityName);

            Func<PolicyTemplate, string> policyTemplateFormatter = (template) =>
            {
                var document = template.Document;

                document = document.Replace("capabilityName", domainEvent.Data.CapabilityName);
                document = document.Replace("capabilityRootId", domainEvent.Data.CapabilityRootId);

                return document;
            };

            _awsIdentityCommandClient.PutRoleAsync(roleName, policyTemplateFormatter);

            return Task.CompletedTask;
        }
    }
}