using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace IAMRoleService.WebApi.Tests.Stubs
{
    public class AmazonSecurityTokenServiceStub : IAmazonSecurityTokenService
    {
        public GetCallerIdentityResponse GetCallerIdentityResponse { get; set; }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public AssumeRoleImmutableCredentials CredentialsFromAssumeRoleAuthentication(string roleArn, string roleSessionName,
            AssumeRoleAWSCredentialsOptions options)
        {
            throw new System.NotImplementedException();
        }

        public IClientConfig Config { get; }
        public Task<AssumeRoleResponse> AssumeRoleAsync(AssumeRoleRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<AssumeRoleWithSAMLResponse> AssumeRoleWithSAMLAsync(AssumeRoleWithSAMLRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<AssumeRoleWithWebIdentityResponse> AssumeRoleWithWebIdentityAsync(AssumeRoleWithWebIdentityRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<DecodeAuthorizationMessageResponse> DecodeAuthorizationMessageAsync(DecodeAuthorizationMessageRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<GetCallerIdentityResponse> GetCallerIdentityAsync(GetCallerIdentityRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(GetCallerIdentityResponse);
        }

        public Task<GetFederationTokenResponse> GetFederationTokenAsync(GetFederationTokenRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<GetSessionTokenResponse> GetSessionTokenAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task<GetSessionTokenResponse> GetSessionTokenAsync(GetSessionTokenRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}