using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PKiK.Server.DB;
using PKiK.Server.Services;
using PKiK.Server.Services.Services;
using PKiK.Server.Shared;
using System;

namespace PKiK.Server.API {
    public class Program {
        private static IConfigurationRoot? configuration;
        private static EnvironmentalSettings? environment;
        public static void Main(string[] args) {
            try {
                var builder = WebApplication.CreateBuilder(args);
                SetConfig();
                if (configuration == null) { throw new Exception("Cannot obtain configuration!"); }
                Config? config = configuration.GetSection("Config").Get<Config>() ?? null;
                if (config == null) { throw new Exception("Cannot read appsettings.json!"); }
                config.ConnectionString = configuration.GetConnectionString("DefaultConnection");
                Config.Set(config);
                builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(config.ConnectionString));
                //Create default user account for deleted accounts
                using (var context = new DataContext()) {
                    context.CreateDefaultUsers();
                }
                //Add singleton config
                builder.Services.AddSingleton(config);
                // Add services to the container.
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IMessageService, MessageService>();
                builder.Services.AddScoped<IKeyService, KeyService>();
                builder.Services.AddScoped<ILoginService, LoginService>();
                builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                var app = builder.Build();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment()) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }catch(Exception exc) {
                Console.WriteLine("Fatal error: "+exc.ToString());
                Console.ReadKey(); //Wait for user to quit
            }
        }
        private static void SetConfig() {
            environment = EnvironmentalSettings.Get();
            configuration = new ConfigurationBuilder()
            .SetBasePath(environment.AppSettingsPath)
            .AddJsonFile("appsettings.json")
            .Build();
        }
    }
}