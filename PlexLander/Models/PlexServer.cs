using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Models
{
    public class PlexServer
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Endpoint { get; set; }
    }
}
