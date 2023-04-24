using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;

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
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Parky API",
                    Version = "1",
                    Description = "Uemy Parky API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "zhendongtt@gmail.com",
                        Name = "Mike Tang",
                        Url = new Uri("https://www.Prosites.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wike/MIT_License")
                    }

                });
            });// this is added by default. Maybe the new version Asp.net core did this.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
            builder.Services.AddScoped<ITrailRepository, TrailRepository>();
            builder.Services.AddAutoMapper(typeof(ParkyMappings));  // add AutoMapper to Service Container
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // added by Default
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                    //options.RoutePrefix = "";

                });// added by Default
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}