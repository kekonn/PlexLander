using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Models
{
    public class PlexAuthentication
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Thumbnail { get; set; }
        public string Token { get; set; }
        public DateTime SessionStart { get; set; }
        public List<PlexServer> Servers { get; set; }
    }
}
