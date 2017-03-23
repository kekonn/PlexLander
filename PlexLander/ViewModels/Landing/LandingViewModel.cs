using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.Landing
{
    public class LandingViewModel
    {
        public IEnumerable<LandingApp> AppList { get; set; }
    }

    public class LandingApp
    {
        /// <summary>
        /// The url we have to call 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// a local url to an image that serves as an icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The display name that is used on the website
        /// </summary>
        public string Name { get; set; }
    }
}
