using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PlexLander.Data
{
    public interface IWhatsNewService : IDisposable
    {
    }

    public class WhatsNewService : IWhatsNewService
    {
        private readonly Plex.IPlexServer plexServer;

        public WhatsNewService(Plex.IPlexServer plexServer)
        {
            this.plexServer = plexServer ?? throw new ArgumentNullException("plexServer");
        }

        public IEnumerable<Episode> GetNewEpisodes(DateTime? start, DateTime? end, int? count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetNewMovies(DateTime? start, DateTime? end, int? count)
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
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WhatsNewService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
