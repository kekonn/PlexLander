using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;

namespace PlexLander.Plex
{
    public interface IPlexServer : IDisposable
    {

        IEnumerable<Movie> GetNewMovies(DateTime? start, DateTime? end, int? count);
        IEnumerable<Episode> GetNewEpisodes(DateTime? start, DateTime? end, int? count);
    }

    public class PlexServer : IPlexServer
    {
        #region Plex header constants
        private const string X_PLEX_TOKEN_HEADER = "X-Plex-Token";
        private const string X_PLEX_PLATFORM_HEADER = "X-Plex-Platform";
        private const string X_PLEX_PLATFORM_VERSION_HEADER = "X-Plex-Platform-Version";
        private const string X_PLEX_CLIENT_VERSION_HEADER = "X-Plex-Client-Identifier";
        private const string X_PLEX_PRODUCT_HEADER = "X-Plex-Product";
        private const string X_PLEX_VERSION_HEADER = "X-Plex-Version";
        private const string X_PLEX_DEVICE_HEADER = "X-Plex-Device";
        private const string X_PLEX_CONTAINER_START_HEADER = "X-Plex-Container-Start";
        #endregion

        private static HttpClient client;
        private string apiEndpoint = "https://app.plex.tv/";

        public PlexServer(Configuration.IConfigurationManager configManager)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (!configManager.IsPlexEnabled)
                throw new ApplicationException("Please enable and configure Plex.");


            if (client == null)
            {
                client = new HttpClient();
            }

            if (string.IsNullOrEmpty(configManager.PlexApp.Endpoint))
            {
                apiEndpoint = configManager.PlexApp.Endpoint;
            }

            client.BaseAddress = new Uri(apiEndpoint);
            SetupRequestHeaders(configManager);
        }

        private void SetupRequestHeaders(Configuration.IConfigurationManager configManager)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(X_PLEX_TOKEN_HEADER, configManager.PlexApp.Token);
            client.DefaultRequestHeaders.Add(X_PLEX_PRODUCT_HEADER, configManager.ApplicationName);
            client.DefaultRequestHeaders.Add(X_PLEX_VERSION_HEADER, configManager.ApplicationVersion);
            client.DefaultRequestHeaders.Add(X_PLEX_DEVICE_HEADER, configManager.DeviceName);

            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetNewMovies(DateTime? start, DateTime? end, int? count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Episode> GetNewEpisodes(DateTime? start, DateTime? end, int? count)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
                
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);        }
        #endregion
    }
}
