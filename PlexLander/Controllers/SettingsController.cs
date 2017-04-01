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
        public async Task<IActionResult> AddApp([Bind("Name","Icon","Url")]App newApp)
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
                //TODO logging
                ModelState.AddModelError("", "Unable to save changes. If the problem persists, please contact your administrator.");
            }
            return PartialView("AddApp", newApp);
        }

        [HttpPost]
        public async Task<IActionResult> SaveApp([Bind("Id","Name","Url","Image")]App app)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Context.Update(app);
                    await Context.SaveChangesAsync();
                    return Json(new AddAppResultViewModel() { Success = true });
                }
            } catch (DbUpdateException)
            {
                return Json(new AddAppResultViewModel() { Success = false, ErrorMessage="fail" });
            }

            return Json(new AddAppResultViewModel() { Success = false, ErrorMessage = "fail" });
        }

        public async Task<IActionResult> DeleteApp(int id)
        {
            try
            {
                App appToDelete = await Context.Apps.Where(a => a.Id == id).SingleOrDefaultAsync();
                if (appToDelete != null)
                {
                    Context.Remove(appToDelete);
                    await Context.SaveChangesAsync();
                }
            } catch(DbUpdateException)
            {
                //TODO logging
                ModelState.AddModelError("", "Unable to save changes. If the problem persists, please contact your administrator.");
            }

            return RedirectToAction("Index");
        }
    }
}
