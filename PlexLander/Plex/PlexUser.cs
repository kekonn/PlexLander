using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Plex
{
    public class PlexUser
    {
        public string Email { get; internal set; }
        public string Username { get; internal set; }
        public string Thumbnail { get; internal set; }
        public string Token { get; internal set; }
    }
}
