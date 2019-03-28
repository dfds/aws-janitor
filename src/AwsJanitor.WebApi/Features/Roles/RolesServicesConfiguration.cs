using Amazon.IdentityManagement;
using Amazon.SecurityToken;
using AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence;
using AwsJanitor.WebApi.Validators;
using Microsoft.Extensions.DependencyInjection;
using Amazon;

namespace AwsJanitor.WebApi.Features.Roles
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

            services.AddTransient<IAwsIdentityCommandClient, AwsIdentityCommandClient>();

            services.AddTransient<IAwsIdentityQueryClient, AwsIdentityQueryClient>();
            
            services.AddSingleton(serviceProvider => PolicyDirectoryLocationFromSolution.Create());
            
            services.AddTransient<IPolicyTemplateRepository, PolicyTemplateRepository>();
        }
    }
}