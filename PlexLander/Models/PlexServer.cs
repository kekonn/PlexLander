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
        public string Name { get; set; }
        public string Uri { get; set; }
    }
}
