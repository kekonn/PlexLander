using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.Landing
{
    public class LandingViewModel : ViewModelBase
    {
        public LandingViewModel(string serverName) : base(serverName)
        {
        }

        public IEnumerable<AppViewModel> AppList { get; set; }
        
    }
}
