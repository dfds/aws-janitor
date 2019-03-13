using Amazon.IdentityManagement;
using Amazon.SecurityToken;
using IAMRoleService.WebApi.Features.Roles.Infrastructure.Persistence;
using IAMRoleService.WebApi.Validators;
using Microsoft.Extensions.DependencyInjection;
using Amazon;

namespace IAMRoleService.WebApi.Features.Roles
{
    public static class RolesServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAmazonIdentityManagementService>(serviceProvider => new AmazonIdentityManagementServiceClient(
                region: serviceProvider.GetRequiredService<RegionEndpoint>()
            ));

            services.AddTransient<IAmazonSecurityTokenService>(serviceProvider => new AmazonSecurityTokenServiceClient(
                region: serviceProvider.GetRequiredService<RegionEndpoint>()
            ));
            
            services.AddTransient<ICreateIAMRoleRequestValidator, CreateIAMRoleRequestValidator>();

            services.AddTransient<IAwsIdentityClient, AwsIdentityClient>();

            services.AddSingleton(serviceProvider => PolicyDirectoryLocationFromSolution.Create());
            
            services.AddTransient<IPolicyRepository, PolicyRepository>();
        }
    }
}