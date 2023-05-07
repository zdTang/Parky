﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ParkyAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;


        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = $"Parky API {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString()
                });
            }

            // These stuff can reference Swigger's documentation
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
               "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
               "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
               "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            // The following is of API documentation with XML file, Ignore them at this point.
            // As the approach this tutorial provided couple with Swigger

            //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //options.IncludeXmlComments(cmlCommentsFullPath);
        }
    }
}
