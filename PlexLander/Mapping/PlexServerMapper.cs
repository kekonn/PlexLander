using System;
using System.Collections.Generic;
using System.Linq;

namespace PlexLander.Mapping
{
    public static class PlexServerMapper
    {
        public static Models.PlexServer MapToEntity(this Plex.PlexServer server)
        {
            return new Models.PlexServer()
            {
                Name = server.Name,
                Uri = GetUri(server)
            };
        }

        public static IEnumerable<Models.PlexServer> MapToEntity(this IEnumerable<Plex.PlexServer> servers)
        {
            return servers.Select(s => s.MapToEntity());
        }

        public static string GetUri(this Plex.PlexServer server)
        {
            var builder = new UriBuilder()
            {
                Scheme = server.Scheme, 
                Host = server.Address,
                Port = int.Parse(server.Port)
            };

            return builder.ToString();
        }

        public static ViewModels.PlexServerViewModel MapToViewModel(this Plex.PlexServer server)
        {
            return new ViewModels.PlexServerViewModel()
            {
                Name = server.Name,
                Url = server.GetUri(),
                Owned = server.Owned
            };
        }

        public static IEnumerable<ViewModels.PlexServerViewModel> MapToViewModel(this IEnumerable<Plex.PlexServer> servers)
        {
            return servers.Select(s => s.MapToViewModel());
        }
    }
}
