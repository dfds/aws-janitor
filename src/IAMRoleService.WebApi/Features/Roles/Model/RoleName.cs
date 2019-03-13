using System;
using System.Text.RegularExpressions;

namespace IAMRoleService.WebApi.Features.Roles.Model
{
    public class RoleName
    {
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
                .Replace(" ", "-")
                .ToLower();
            
            roleName = Regex.Replace(roleName, "[^0-9a-zA-Z+=,.@_-]+", "");

            return new RoleName(roleName);
        }

        private readonly string _name;

        public RoleName(string name)
        {
            _name = name;
        }

      
        public static implicit operator string(RoleName roleName)
        {
            return roleName?._name;
        }

        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case RoleName roleName:
                    return _name.Equals(roleName._name);
                case string item:
                    return _name.Equals(item);
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}