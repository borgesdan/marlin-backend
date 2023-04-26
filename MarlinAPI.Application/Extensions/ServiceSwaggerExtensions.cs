using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MarlinAPI.Application.Extensions
{
    public static class ServiceCollectionSwagger
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, AssemblyName applicationName)
        {
            services.AddSwaggerGen(options =>
            {
                SetupSwaggerDoc(options, applicationName);               

                string path = applicationName.Name + ".xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, path));
            });
            return services;
        }

        private static void SetupSwaggerDoc(SwaggerGenOptions options, AssemblyName applicationName)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = GetSwaggerDocTitle(applicationName),
                Description = "Documentação detalhada para utilização das APIs REST disponíveis no projeto " + applicationName.Name,
                Contact = new OpenApiContact
                {
                    Email = "borges_santos89@hotmail.com",
                    Name = "Danilo Borges",
                    Url = new Uri("http://www.dandev.com.br"),
                }
            });
        }

        private static string GetSwaggerDocTitle(AssemblyName applicationName)
        {
            return (applicationName?.Name ?? string.Empty)!.Split(".").First();
        }
    }
}
