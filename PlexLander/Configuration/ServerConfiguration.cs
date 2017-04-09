using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Configuration
{
    public class ServerConfiguration
    {
        public string ServerName { get; set; }
        //plex settings
        public bool EnablePlex { get; set; } = false;
        public string PlexEndpoint { get; set; }
        public string PlexUrl { get; set; }
        public string PlexToken { get; set; }
        public string PlexIcon { get; set; }
        //whatsnewsettings
        public bool EnableWhatsNew { get; set; } = false;
        public string WhatsNewIcon { get; set; }
        //Radarr settings
        public bool EnableRadarr { get; set; } = false;
        public string RadarrEndpoint { get; set; }
        //sonarr settings
        public bool EnableSonarr { get; set; } = false;
        public string SonarrEndpoint { get; set; }
    }
}
