using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.Settings
{
    public class PlexServerSettingsViewModel
    {
        [DisplayName("Enable")]
        public bool IsEnabled { get; set; }
        public string Token { get; set; }
        [DisplayName("Authenticated")]
        public bool HasAuthentication { get; set; }
        public PlexAuthenticationResultViewModel AuthenticationResult { get; set; }

        public List<PlexServerViewModel> PlexServers { get; set; }
    }
}
