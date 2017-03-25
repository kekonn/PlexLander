﻿using PlexLander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.Settings
{
    public class SettingsIndexViewModel : ViewModelBase
    {
        public ICollection<App> Apps { get; set; }

        public SettingsIndexViewModel(string serverName) : base(serverName)
        {
        }
    }
}