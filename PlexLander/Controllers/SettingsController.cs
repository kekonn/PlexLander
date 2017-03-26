using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlexLander.Configuration;
using PlexLander.Data;
using PlexLander.ViewModels.Settings;
using PlexLander.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlexLander.Controllers
{
    public class SettingsController : PlexLanderBaseController
    {
        private const string ControllerName = "Settings";

        protected IQueryable<App> NonTrackingApps
        {
            get
            {
                return Context.Apps.AsNoTracking();
            }
        }

        public SettingsController(PlexLanderContext context, IOptions<ServerConfiguration> config) : base(context,config)
        { }

        // GET: /Settings/
        public async Task<IActionResult> Index()
        {
            return View(new SettingsIndexViewModel(ServerName) { ActiveControllerName = ControllerName, Apps = await Context.Apps.ToListAsync() });
        }

        //POST: /Settings/AddApp
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> AddApp(App newApp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (newApp.Url.First() != '/')
                    {
                        newApp.Url = String.Format("/{0}", newApp.Url);
                    }
                    Context.Add(newApp);
                    await Context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. If the problem persists, please contact your administrator.");
            }
            return PartialView("AddApp", newApp);
        }
    }
}
