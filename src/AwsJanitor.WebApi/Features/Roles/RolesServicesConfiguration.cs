using Amazon.SecurityToken;
using AwsJanitor.WebApi.Features.Roles.Infrastructure.Persistence;
using AwsJanitor.WebApi.Validators;
using Microsoft.Extensions.DependencyInjection;
using Amazon;
using Amazon.IdentityManagement;
using AwsJanitor.WebApi.Features.Roles.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AwsJanitor.WebApi.Features.Roles
{
    public static class RolesServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
           
            services.TryAddTransient<IAmazonSecurityTokenService>(serviceProvider => new AmazonSecurityTokenServiceClient(
                region: serviceProvider.GetRequiredService<RegionEndpoint>()
            ));

            services.AddTransient<IAmazonIdentityManagementService, AmazonIdentityManagementServiceClient>();
            
            services.AddTransient<IAmazonIdentityManagementServiceWrapper, AmazonIdentityManagementServiceWrapper>();

            services.AddTransient<IIdentityManagementServiceClient, IdentityManagementServiceClient>();
            
            services.TryAddTransient<IAwsIdentityCommandClient, AwsIdentityCommandClient>();

            
            services.TryAddTransient<ICreateIAMRoleRequestValidator, CreateIAMRoleRequestValidator>();

            services.AddTransient<IAwsIdentityQueryClient, AwsIdentityQueryClient>();
            
            services.AddSingleton(serviceProvider => PolicyDirectoryLocationFromSolution.Create());
            
            services.AddTransient<IPolicyTemplateRepository, PolicyTemplateRepository>();
        }
    }
}