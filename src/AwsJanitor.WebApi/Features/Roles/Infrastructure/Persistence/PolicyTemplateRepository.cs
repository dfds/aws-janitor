using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles.Model;


namespace AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence
{
    public class PolicyTemplateRepository : IPolicyTemplateRepository
    {
        private readonly PolicyDirectoryLocation _policyDirectoryLocation;

        public PolicyTemplateRepository(PolicyDirectoryLocation policyDirectoryLocation)
        {
            _policyDirectoryLocation = policyDirectoryLocation;
        }

        public async Task<IEnumerable<PolicyTemplate>> GetLatestAsync()
        {
            var policyFiles = GetAllPolicyFiles();


            var policyTasks = policyFiles.Select(async filePath =>
            {
                {
                    var policyDocument = await ReadFileAsync(filePath);
                    var policyName = Path.GetFileNameWithoutExtension(filePath);
                    
                    var policy = new PolicyTemplate(
                        policyName,
                        policyDocument
                    );

                    return policy;
                }
            }).ToArray();
            
            var policies = await Task.WhenAll(policyTasks);


            return policies;
        }

        
        public IEnumerable<string> GetAllPolicyFiles()
        {
            var jsonFiles = Directory.EnumerateFiles(
                _policyDirectoryLocation, 
                "*.json", 
                SearchOption.TopDirectoryOnly
            );


            return jsonFiles;
        }

        
        public async Task<string> ReadFileAsync(string filePath)
        {
            var fileContent = await File.ReadAllTextAsync(filePath);


            return fileContent;
        }
    }
}