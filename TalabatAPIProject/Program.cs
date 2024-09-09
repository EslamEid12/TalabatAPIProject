using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Helpers;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;
using TalabatAPIProject.Error;
using TalabatAPIProject.Extension;
using TalabatAPIProject.Middlwares;

namespace TalabatAPIProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSwaggerService();

            builder.Services.AddDbContext<StoreDBContext>(options =>{
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(optionsbuilder => {
                optionsbuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            //builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            //{
            //    var connection = builder.Configuration.GetConnectionString("Redis");
            //    return ConnectionMultiplexer.Connect(connection);
            //});

            builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var configurationOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                configurationOptions.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            // ApplicationServicesExtensions.AddApplicationServises((builder.Services));
            builder.Services.AddApplicationServises();
            builder.Services.IdentityServices(builder.Configuration);

            var app = builder.Build();

            var scop = app.Services.CreateScope();
            var Services = scop.ServiceProvider;
            
           
            var LoggeFactory=Services.GetRequiredService<ILoggerFactory>();
            try
            {

                var DbContext = Services.GetRequiredService<StoreDBContext>();
                await DbContext.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(DbContext);

                var identityContext = Services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync();

                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);

                

              
            }
            catch (Exception ex)
            {
                var Logger = LoggeFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured during apply the Migration");
            }

            app.UseMiddleware<EexceptionMiddlware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlware();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
          

            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}