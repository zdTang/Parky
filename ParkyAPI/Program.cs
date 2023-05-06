using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace ParkyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
            //    {
            //        Title = "Parky API",
            //        Version = "1",
            //        Description = "Uemy Parky API",
            //        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //        {
            //            Email = "zhendongtt@gmail.com",
            //            Name = "Mike Tang",
            //            Url = new Uri("https://www.Prosites.com")
            //        },
            //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //        {
            //            Name = "MIT License",
            //            Url = new Uri("https://en.wikipedia.org/wike/MIT_License")
            //        }

            //    });
            //});// this is added by default. Maybe the new version Asp.net core did this.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
            builder.Services.AddScoped<ITrailRepository, TrailRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            //Why we retrieve value from appSettings.json like this complicated.
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings?.Secret);
            //notice to install the right Version: Microsoft.AspNetCore.Authentication.JwtBearer;
            //Here to install relevent classes to support this kind of Authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Just a string
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //Just a string
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,  // Once deploy to Production, need set it as True
                    ValidateAudience = false // Once deploy to Production, need set it as True
                };
            });

            builder.Services.AddAutoMapper(typeof(ParkyMappings));  // add AutoMapper to Service Container
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // added by Default
                app.UseSwaggerUI(options =>
                {
                    foreach (var desc in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                        options.RoutePrefix = "";
                    }
                });
                //app.UseSwaggerUI(options =>
                //{
                //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                //    //options.RoutePrefix = "";

                //});// added by Default
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}