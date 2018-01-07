using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Data
{
    public class PlexServer
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public string Endpoint { get; set; }
    }
}
