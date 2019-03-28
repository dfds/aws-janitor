namespace AwsJanitor.WebApi.Features.Roles.Model
{
    public class PolicyTemplate
    {
        public PolicyTemplate(string name, string document)
        {
            Name = name;
            Document = document;
        }
        
        public string Name { get; }
        public string Document { get; }
    }
}