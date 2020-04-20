using System;

namespace AwsJanitor.WebApi.Features.Roles.Model
{
    public class Policy
    {
        private Policy(string name, string document)
        {
            Name = name;
            Document = document;
        }
        
        public string Name { get; }
        public string Document { get; }

        public static Policy Create(PolicyTemplate policyTemplate, Func<PolicyTemplate, string> policyTemplateFormatter = default)
        {
            var policy = new Policy(
                name: policyTemplate.Name, 
                document: policyTemplateFormatter != default ? policyTemplateFormatter(policyTemplate) : policyTemplate.Document
            );

            return policy;
        }
    }
}