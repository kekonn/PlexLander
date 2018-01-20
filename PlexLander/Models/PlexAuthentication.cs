using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlexLander.Models
{
    public class PlexAuthentication
    {
        [Key]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Thumbnail { get; set; } = String.Empty;
        [Key]
        public string Token { get; set; }
        public DateTime SessionStart { get; set; }
        public List<PlexServer> Servers { get; set; }
    }
}
