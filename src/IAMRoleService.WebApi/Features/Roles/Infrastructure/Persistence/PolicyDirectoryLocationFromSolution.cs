using System.IO;

namespace IAMRoleService.WebApi.Features.Roles.Infrastructure.Persistence
{
    public static class PolicyDirectoryLocationFromSolution
    {
        public static PolicyDirectoryLocation Create()
        {
            var baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var filesFolder = Path.Combine(baseFolder, "Features/Roles/Infrastructure/Persistence/Policies");
            
            
            return new PolicyDirectoryLocation(filesFolder);
        }
    }
}