using System;

namespace AwsJanitor.WebApi.Infrastructure.Messaging
{
    public class MessagingException : Exception
    {
        public MessagingException(string message) : base(message)
        {
        }
    }
}