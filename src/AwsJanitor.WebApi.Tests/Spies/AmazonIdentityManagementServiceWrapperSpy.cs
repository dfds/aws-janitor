using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using AwsJanitor.WebApi.Features.Roles.Infrastructure;

namespace AwsJanitor.WebApi.Tests.Spies
{
    public class AmazonIdentityManagementServiceWrapperSpy : IAmazonIdentityManagementServiceWrapper
    {
        public Task<DeleteRolePolicyResponse> DeleteRolePolicyAsync(DeleteRolePolicyRequest deleteRolePolicyRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<ListRolePoliciesResponse> ListRolePoliciesAsync(ListRolePoliciesRequest listRolePoliciesRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<DeleteRoleResponse> DeleteRoleAsync(DeleteRoleRequest deleteRoleRequest)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Func<GetRolePolicyResponse> GetPolicyAsyncResponse;
        public Task<GetRolePolicyResponse> GetPolicyAsync(GetRolePolicyRequest getRolePolicyRequest)
        {
           return Task.FromResult(GetPolicyAsyncResponse());
        }

        public List<PutRolePolicyRequest> PutRolePolicyRequests { get; } = new List<PutRolePolicyRequest>();

        public Task<PutRolePolicyResponse> PutRolePolicyAsync(PutRolePolicyRequest rolePolicyRequest)
        {
            PutRolePolicyRequests.Add(rolePolicyRequest);
            return Task.FromResult(new PutRolePolicyResponse());
        }
    }
}