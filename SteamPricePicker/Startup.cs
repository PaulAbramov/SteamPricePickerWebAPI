using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Data;

namespace SteamPricePicker
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //Registriere den Context in der ServiceCollection.
            //Benutze dafür den SQLServer der in "appsettings.json" unter "DefaultConnection" definiert ist
            services.AddDbContext<ItemContext>(_options => _options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //var test = loggerFactory.CreateLogger("errorhandler");
            //
            //loggerFactory.AddFile("logs/log.txt");
            //
            app.UseMvc();
            //
            //Dictionary<string, string> connectionStrings = new Dictionary<string, string>();
            //connectionStrings.Add("DefaultConnection", Configuration.GetConnectionString("DefaultConnection"));
            //DBContextFactory.SetConnectionString(connectionStrings);
            //
            //DBInitializer.Initialize(test);
        }
    }
}
