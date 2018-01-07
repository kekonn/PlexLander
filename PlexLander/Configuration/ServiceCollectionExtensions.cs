using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<Data.IAppRepository, Data.AppRepository>();
        }

        public static IServiceCollection AddServers(this IServiceCollection services)
        {
            return services.AddScoped<Plex.IPlexService, Plex.PlexService>();
        }
    }
}
