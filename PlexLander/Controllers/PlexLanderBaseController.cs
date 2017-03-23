using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Controllers
{
    public class PlexLanderBaseController : Controller
    {
        private readonly AppSettings _appSettings;

        public AppSettings AppSettings
        {
            get
            {
                return _appSettings;
            }
        }

        public PlexLanderBaseController(IOptions<AppSettings> options) : base()
        {
            _appSettings = options.Value;
        }
    }
}
