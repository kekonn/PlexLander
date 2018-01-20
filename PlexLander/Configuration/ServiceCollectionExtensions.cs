using Microsoft.Extensions.DependencyInjection;

namespace PlexLander.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<Data.IAppRepository, Data.AppRepository>()
                .AddScoped<Data.IPlexSessionRepository, Data.PlexSessionRepository>();
        }

        public static IServiceCollection AddServers(this IServiceCollection services)
        {
            return services.AddScoped<Plex.IPlexService, Plex.PlexService>();
        }
    }
}
