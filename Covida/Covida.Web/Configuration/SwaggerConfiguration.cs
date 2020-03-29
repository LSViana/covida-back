using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Covida.Web.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this IServiceCollection @this)
        {
            @this.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerExcludeSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Covida",
                    Version = "v1",
                    Description = "An API for collaboration during COVID-19 pandemic.",
                    Contact = new OpenApiContact
                    {
                        Name = "Lucas Viana",
                        Email = "lv201122@gmail.com",
                        Url = new Uri("https://lsviana.github.io/"),
                    }
                });
                c.CustomSchemaIds((type) => type.FullName);
            });
        }
    }

    public class SwaggerExcludeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var propertiesToExclude = context.Type.GetProperties()
                .Where(x => x.GetCustomAttribute<SwaggerExcludeAttribute>() != null)
                .ToArray();
            //
            foreach (var property in propertiesToExclude)
            {
                if (schema.Properties.ContainsKey(property.Name))
                {
                    schema.Properties.Remove(property.Name);
                }
            }
        }
    }

    /// <summary>
    /// Used to remove properties from Swagger Docs
    /// </summary>
    public class SwaggerExcludeAttribute : Attribute { }
}
