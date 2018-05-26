using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels.WhatsNew
{
    public class IndexViewModel : ViewModelBase
    {
        public List<PlexServerViewModel> Servers { get; set; }
        public PlexServerViewModel SelectedServer { get; set; }

        public IndexViewModel(string serverName) : base(serverName)
        {
        }
    }
}
