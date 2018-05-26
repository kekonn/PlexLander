using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels
{
    public class PlexServerViewModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public bool Owned { get; set; } = false;
    }
}
