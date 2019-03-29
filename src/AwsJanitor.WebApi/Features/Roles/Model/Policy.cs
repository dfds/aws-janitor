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

        
        public static Policy Create(PolicyTemplate policyTemplate, CapabilityName capabilityName)
        {
            var policy = new Policy(
                name: policyTemplate.Name, 
                document: policyTemplate.Document.Replace("capabilityName", capabilityName.ToString().ToLower())
            );

            
            return policy;
        }
    }
}