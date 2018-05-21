using System;

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
    }
}
