using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Covida.Infrastructure.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; }
        public object Details { get; }

        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HttpException(int httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int)httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(int httpStatusCode, object details)
        {
            StatusCode = httpStatusCode;
            Details = details;
        }

        public HttpException(HttpStatusCode httpStatusCode, object details)
        {
            StatusCode = (int)httpStatusCode;
            Details = details;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = (int)httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = (int)httpStatusCode;
        }
    }
}
