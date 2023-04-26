
using MarlinAPI.Application.Extensions;
using MarlinAPI.Repository;
using MarlinAPI.Repository.Context;
using MarlinAPI.Service;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MarlinAPI.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerDocumentation(assemblyName);            
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<StudentRepository>();
            builder.Services.AddScoped<ClassRepository>();

            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<ClassService>();

            

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("local"), b => b.MigrationsAssembly(assemblyName.Name)));
            

            //Adiciona Cors Policy padrão.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}