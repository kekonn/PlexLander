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
using PlexLander.Plex;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlexLander.Controllers
{
    [RequireHttps]
    public class SettingsController : PlexLanderBaseController
    {
        private readonly IAppRepository _appRepo;
        private readonly IPlexService _plexService;
        private readonly IPlexSessionRepository _plexSessionRepo;

        public SettingsController(IPlexService plexService, IAppRepository appRepo, 
            IPlexSessionRepository plexSessionRepo, IConfigurationManager configManager) : base(configManager)
        {
            _appRepo = appRepo;
            _plexService = plexService;
            _plexSessionRepo = plexSessionRepo;
        }

        // GET: /Settings/
        public IActionResult Index(PlexAuthenticationResultViewModel plexLoginResult = null)
        {
            return View(new SettingsIndexViewModel(ServerName)
            {
                Apps = _appRepo.ListAll(),
                PlexServerSettingsViewModel = CreatePlexServerSettingsViewModel(plexLoginResult)
            });
        }

        //POST: /Settings/SavePlexServerSettings
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> SavePlexServerSettings(bool isEnabled, string token)
        {
            throw new NotImplementedException();
        }

        //POST: /Settings/AddApp
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public IActionResult AddApp([Bind("Name", "Icon", "Url")]App newApp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (newApp.Url.First() != '/')
                    {
                        newApp.Url = String.Format("/{0}", newApp.Url);
                    }
                    _appRepo.Add(newApp);
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
        public IActionResult SaveApp([Bind("Id", "Name", "Url", "Icon")]App app)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _appRepo.Update(app);
                    return PartialView("_AppRow", app);
                }
            }
            catch (DbUpdateException)
            {
                return Json(new { ErrorMessage = "fail" });
            }

            return Json(new { ErrorMessage = "fail" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlexAuthentication(string username, string password)
        {
            var loginResult = await _plexService.Login(username, password)
            if (loginResult.Succes)
            {
                var user = loginResult.User;
                _plexSessionRepo.DeleteOldSessions();
                var sessionsWithEmailQuery = _plexSessionRepo.GetSessionsForEmail(loginResult.User.Email);
                if (sessionsWithEmailQuery.Any())
                {

                    var session = sessionsWithEmailQuery.Where(s => s.Token == user.Token && s.Username == user.Username);

                }
            }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult DeleteApp(int id)
        {
            try
            {
                _appRepo.Remove(id);
            }
            catch (DbUpdateException)
            {
                //TODO logging
                ModelState.AddModelError("", "Unable to save changes. If the problem persists, please contact your administrator.");
            }

            return RedirectToAction("Index");
        }

        private PlexServerSettingsViewModel CreatePlexServerSettingsViewModel(PlexAuthenticationResultViewModel plexLoginResult)
        {
            return new PlexServerSettingsViewModel()
            {
                IsEnabled = ConfigManager.IsPlexEnabled,
                Token = ConfigManager.PlexApp.Token,
                HasAuthentication = _plexService.HasValidLogin,
                AuthenticationResult = plexLoginResult
            };
        }
    }
}
