using Covida.Infrastructure.Definitions;
using Covida.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Covida.Web.Middlewares.HttpExceptionHandling
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment hostEnvironment;

        public HttpExceptionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            this.next = next;
            this.hostEnvironment = hostEnvironment;
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
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                if (hostEnvironment.IsDevelopment())
                {
                    throw;
                }
                else
                {
                    // TODO (LSViana) Add logging for this exception
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo(ErrorMessages.InternalServerError)));
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
