using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Configuration
{
    public class ServerConfiguration
    {
        public string ServerName { get; set; }
        public bool EnablePlex { get; set; } = false;
        public string PlexEndpoint { get; set; }
        public string PlexToken { get; set; }
        public bool EnableWhatsNew { get; set; } = false;
        public bool EnableRadarr { get; set; } = false;
        public string RadarrEndpoint { get; set; }
        public bool EnableSonarr { get; set; } = false;
        public string SonarrEndpoint { get; set; }
    }
}
