using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.ViewModels.Landing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Data;
using PlexLander.Models;

namespace PlexLander.Controllers
{
    public class LandingController : Controller
    {
        private readonly PlexLanderContext _context;

        public LandingController(PlexLanderContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new LandingViewModel() { AppList = new List<App>(_context.Apps.AsEnumerable()) };
            return View(viewModel);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
