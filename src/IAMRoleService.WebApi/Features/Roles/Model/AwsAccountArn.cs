namespace IAMRoleService.WebApi.Models
{
    public class AwsAccountArn
    {
        public AwsAccountArn(string accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }

        public override string ToString()
        {
            return $"arn:aws:iam::{AccountNumber}";
        }
    }
}