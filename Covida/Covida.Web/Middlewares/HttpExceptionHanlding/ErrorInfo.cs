using System;

namespace Covida.Web.Middlewares.HttpExceptionHandling
{
    internal class ErrorInfo
    {
        public string Message { get; }

        public ErrorInfo(string message)
        {
            Message = message;
        }

        public override bool Equals(object obj)
        {
            return obj is ErrorInfo other &&
                   Message == other.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Message);
        }
    }
}
