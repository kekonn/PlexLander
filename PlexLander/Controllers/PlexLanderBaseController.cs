using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels;

namespace PlexLander.Controllers
{
    public abstract class PlexLanderBaseController : Controller
    {
        protected string ServerName { get; private set; }

        private readonly PlexLanderContext _context;
        private IOptions<ServerConfiguration> config;

        public PlexLanderContext Context => _context;

        public PlexLanderBaseController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base()
        {
            _context = context;
            ServerName = config.Value.ServerName;
        }

        public PlexLanderBaseController(IOptions<ServerConfiguration> config)
        {
            this.config = config;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as PlexLanderBaseController;

            if (controller != null && (context.Controller as PlexLanderBaseController).ViewData.Model == null)
            {
                controller.ViewData.Model = new BasicViewModel(ServerName);
            }
            base.OnActionExecuted(context);
        }

        private sealed class BasicViewModel : ViewModelBase
        { 
            public BasicViewModel(string serverName) : base(serverName)
            {
            }
        }
    }
}
