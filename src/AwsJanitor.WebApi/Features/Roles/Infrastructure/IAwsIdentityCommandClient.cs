using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Features.Roles
{
    public interface IAwsIdentityCommandClient
    {
        Task <Role>PutRoleAsync(RoleName roleName, Func<PolicyTemplate, string> policyTemplateFormatter = default);

        Task SyncRole(RoleName roleName);
    }
}