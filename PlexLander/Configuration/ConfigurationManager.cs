using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PlexLander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Configuration
{
    public class ConfigurationManager
    {
        public bool IsPlexEnabled { get; private set; }
        public bool IsRadarrEnabled { get; private set; }
        public bool IsSonarrEnabled { get; private set; }
        public bool IsWhatsNewEnabled { get; private set; }
        public string ServerName { get; private set; }

        private BuiltInApp _plexApp;
        private BuiltInApp _radarrApp;
        private BuiltInApp _sonarrApp;
        private BuiltInApp _whatsNewApp;

        public ConfigurationManager(IOptions<ServerConfiguration> config)
        {
            ParseConfiguration(config.Value);
        }

        private void ParseConfiguration(ServerConfiguration config)
        {
            ServerName = config.ServerName;
            IsPlexEnabled = config.EnablePlex;
            IsRadarrEnabled = config.EnableRadarr;
            IsSonarrEnabled = config.EnableSonarr;
            IsWhatsNewEnabled = config.EnableWhatsNew;

            if (IsPlexEnabled)
            {
                _plexApp = new BuiltInApp("Plex", config.PlexIcon, config.PlexUrl, config.PlexEndpoint, config.PlexToken,-1);
            }

            if (IsWhatsNewEnabled)
            {
                _whatsNewApp = new BuiltInApp("What's New", config.WhatsNewIcon, "/WhatsNew",id:-2);
            }
        }

        public IEnumerable<BuiltInApp> ListAll()
        {
            var appList = new List<BuiltInApp>(4);
            if (_plexApp != null)
            {
                appList.Add(_plexApp);
            }
            if (_whatsNewApp != null)
            {
                appList.Add(_whatsNewApp);
            }

            appList.Sort(new BuiltInAppIdComparer());
            return appList.AsEnumerable();
        }

        private class BuiltInAppIdComparer : IComparer<BuiltInApp>
        {
            public int Compare(BuiltInApp x, BuiltInApp y)
            {
                //Assume built in app ids are negative, but compare as if they weren't
                return -(x.Id) - -(y.Id);
            }
        }
    }
}
