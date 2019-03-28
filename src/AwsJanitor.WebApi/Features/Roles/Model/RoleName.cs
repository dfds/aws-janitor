using System;
using System.Text.RegularExpressions;
using AwsJanitor.WebApi.Shared.Model;

namespace AwsJanitor.WebApi.Features.Roles.Model
{
    public class RoleName : StringSubstitutable
    {
        // Rules
        // https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_iam-limits.html

        public static RoleName Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException($"{nameof(roleName)} has a minimum length of 1");
            }

            if (64 < roleName.Length)
            {
                throw new ArgumentException($"{nameof(roleName)} has a maximum length of 64");
            }

            roleName = roleName
                .Replace(" ", "-");

            roleName = Regex.Replace(roleName, "[^0-9a-zA-Z+=,.@_-]+", "");

            return new RoleName(roleName);
        }

        public RoleName(string name) : base(name)
        {
        }
    }
}