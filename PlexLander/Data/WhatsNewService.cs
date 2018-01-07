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
        private readonly Plex.IPlexService _plexServer;

        public WhatsNewService(Plex.IPlexService plexServer)
        {
            _plexServer = plexServer ?? throw new ArgumentNullException("plexServer");
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
                    _plexServer.Dispose();
                }
                
                disposedValue = true;
            }
        }

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
