using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PlexLander.Data;
using PlexLander.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace PlexLander
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //Configure server
            services.Configure<ServerConfiguration>(Configuration.GetSection("ServerConfiguration"));
            //Add EFCore
            services.AddDbContext<PlexLanderContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Add caching
            services.AddMemoryCache();

            //add own components
            services.AddSingleton<IConfigurationManager,ConfigurationManager>();
            services.AddSingleton<Plex.IPlexService, Plex.PlexService>();
            services.AddSingleton<IWhatsNewService, WhatsNewService>();
            services.AddRepositories();
            services.AddServers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, PlexLanderContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            DbInitializer.Initialize(context);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Landing}/{action=Index}");
            });
        }
    }

    public class PlexLanderContextFactory : IDesignTimeDbContextFactory<PlexLanderContext>
    {
        public PlexLanderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlexLanderContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PlexLander;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PlexLanderContext(optionsBuilder.Options);
        }
    }
}
