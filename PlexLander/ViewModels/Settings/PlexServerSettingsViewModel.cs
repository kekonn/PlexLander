using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.Settings
{
    public class PlexServerSettingsViewModel
    {
        [DisplayName("Enable?")]
        public bool IsEnabled { get; set; }
        public string Token { get; set; }
    }
}
