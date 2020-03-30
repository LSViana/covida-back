using System;
using System.Collections.Generic;
using System.Text;

namespace Covida.Infrastructure.Definitions
{
    public static class ErrorMessages
    {
        public const string InvalidToken = "Invalid token, try logging in again to generate a new one";
        public const string UnknownErrorContactSupport = "Unknown error, contact the support";
        public const string InternalServerError = "Internal server error";
        public const string AnotherVolunteerAlreadyAnswered = "Another volunteer already answered";

        public static string EntityNotFound<T>()
        {
            return $"Entity {typeof(T)?.Name ?? "(unknown name)"} not found";
        }
    }
}
