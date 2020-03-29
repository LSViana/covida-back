using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Covida.Web.Middlewares.HttpExceptionHandling
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                context.Response.ContentType = "application/json";
                if (httpException.Details != null)
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(httpException.Details));
                }
                else if(httpException.Message != null)
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(httpException.Message)));
                } else
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ErrorMessages.UnknownErrorContactSupport)));
                }
            }
        }
    }

    public static class HttpExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder @this)
        {
            @this.UseMiddleware<HttpExceptionMiddleware>();
            //
            return @this;
        }
    }
}
