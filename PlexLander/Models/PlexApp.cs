using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Models
{
    public class PlexApp : App
    {
        private bool isEnabled = false;

        public string Token { get; set; }


        public PlexApp()
        {
            Name = "Plex";
            Icon = "plex.png";
        }
    }
}
