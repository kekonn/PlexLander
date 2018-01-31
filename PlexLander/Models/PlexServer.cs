using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Models
{
    public class PlexServer
    {
        [Key]
        public int Id { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Endpoint { get; set; }
    }
}
