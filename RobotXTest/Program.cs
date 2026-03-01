using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RobotXTest.BusinessLogic.Core.Interface;
using RobotXTest.BusinessLogic.Services;
using RobotXTest.DataAccess.DbContexts;

namespace RobotXTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                else
                {
                    Console.WriteLine($"XML file for Swagger not found: {xmlPath}");
                }
            });

            #region SERVICES
            builder.Services.AddScoped<IClientService, ClientService>();
            #endregion

            builder.Services.AddCors(o => o.AddPolicy("React", p =>
                p.WithOrigins("http://localhost:5173")
                 .AllowAnyHeader()
                 .AllowAnyMethod()));

            builder.Services.AddDbContext<RobotXTestContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("React");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
