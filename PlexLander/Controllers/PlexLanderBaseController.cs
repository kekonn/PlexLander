using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.ViewModels;

namespace PlexLander.Controllers
{
    public abstract class PlexLanderBaseController : Controller
    {
        protected string ServerName { get; private set; }

        public PlexLanderBaseController(IOptions<ServerConfiguration> config) : base()
        {
            ServerName = config.Value.ServerName;
        }
    }
}
