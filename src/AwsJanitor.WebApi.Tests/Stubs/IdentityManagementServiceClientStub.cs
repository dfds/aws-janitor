using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwsJanitor.WebApi.Features.Roles;
using AwsJanitor.WebApi.Features.Roles.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class IdentityManagementServiceClientStub : IIdentityManagementServiceClient
    {
        public Task DeleteRolePoliciesAsync(RoleName roleName, IEnumerable<string> namesOfPoliciesToDelete)
        {
            return Task.CompletedTask;
        }

        public Task DeleteRoleAsync(RoleName roleName)
        {
            throw new NotImplementedException();
        }
    }
}