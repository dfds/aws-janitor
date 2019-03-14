using Amazon.SecurityToken.Model;

namespace AwsJanitor.WebApi.Tests.Stubs
{
    public class AmazonSecurityTokenServiceStubBuilder
    {
        private readonly AmazonSecurityTokenServiceStub  AmazonSecurityTokenServiceStub= new AmazonSecurityTokenServiceStub();
        
        public AmazonSecurityTokenServiceStub WithGetCallerIdentityResponse(
            GetCallerIdentityResponse getCallerIdentityResponse)
        {
            AmazonSecurityTokenServiceStub.GetCallerIdentityResponse = getCallerIdentityResponse;

            return AmazonSecurityTokenServiceStub;
        }
    }
}