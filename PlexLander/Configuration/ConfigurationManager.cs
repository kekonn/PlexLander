using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlexLander.Configuration
{
    public interface IConfigurationManager
    {
        //properties
        bool IsPlexEnabled { get; }
        BuiltInApp PlexApp { get; }
        bool IsRadarrEnabled { get; }
        bool IsSonarrEnabled { get; }
        bool IsWhatsNewEnabled { get; }
        string ServerName { get; }
        string ApplicationVersion { get; }
        string ApplicationName { get; }
        string DeviceName { get; }
        string PlatformName { get; }
        string PlatformVersion { get; }

        //methods

        /// <summary>
        /// Gets a list of all the built in applications.
        /// </summary>
        /// <returns>an IEnumerable&lt;BuiltInApp&gt; of apps.</returns>
        IEnumerable<BuiltInApp> ListAll();
    }

    public class ConfigurationManager : IConfigurationManager
    {
        public bool IsPlexEnabled { get; private set; }
        public BuiltInApp PlexApp
        {
            get
            {
                if (IsPlexEnabled)
                    return _plexApp;
                else
                    throw new InvalidOperationException("Please enable and configure Plex first.");
            }
        }
        public bool IsRadarrEnabled { get; private set; }
        public bool IsSonarrEnabled { get; private set; }
        public bool IsWhatsNewEnabled { get; private set; }
        public string ServerName { get; private set; }
        public string ApplicationVersion
        {
            get
            {
                return PlatformServices.Default.Application.ApplicationVersion;
            }
        }
        public string ApplicationName
        {
            get
            {
                return PlatformServices.Default.Application.ApplicationName;
            }
        }
        public string DeviceName
        {
            get
            {
                return "PlexLanderServer"; //TODO: detect actual "device" name
            }
        }
        public string PlatformName
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            }
        }
        public string PlatformVersion
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
            }
        }

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
                if (!string.IsNullOrEmpty(config.PlexEndpoint))
                {
                    _plexApp = new BuiltInApp("Plex", config.PlexIcon, config.PlexUrl, config.PlexEndpoint, config.PlexToken, -1);
                }
                else
                {
                    _plexApp = new BuiltInApp(name: "Plex", icon: config.PlexIcon, url: config.PlexUrl, token: config.PlexToken, id: -1);
                }
            }

            if (IsWhatsNewEnabled)
            {
                _whatsNewApp = new BuiltInApp("What's New", config.WhatsNewIcon, "/Index", id: -2, endpoint: "WhatsNew");
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
            //todo: add the rest of the builtin apps
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
