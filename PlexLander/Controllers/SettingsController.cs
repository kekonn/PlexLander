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
        private readonly TimeSpan _maxSessionAge = TimeSpan.FromDays(10d);

        public SettingsController(IPlexService plexService, IAppRepository appRepo,
            IPlexSessionRepository plexSessionRepo, IConfigurationManager configManager) : base(configManager)
        {
            _appRepo = appRepo;
            _plexService = plexService;
            _plexSessionRepo = plexSessionRepo;
        }

        // GET: /Settings/
        [HttpGet()]
        public IActionResult Index([Bind("Succes", "Error")]PlexAuthenticationResultViewModel authResult)
        {
            var vm = new SettingsIndexViewModel(ServerName)
            {
                Apps = _appRepo.ListAll()
            };
            if (!authResult.Succes && authResult.Error == null) // default values, so index was called without parameters
            {
                vm.PlexServerSettingsViewModel = CreatePlexServerSettingsViewModel(null);
                return View(vm);
            }
            else
            {
                vm.PlexServerSettingsViewModel = CreatePlexServerSettingsViewModel(authResult);
                return View(vm);
            }
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
            var loginResult = await _plexService.Login(username, password);
            if (loginResult.Succes)
            {
                var user = loginResult.User;
                _plexSessionRepo.DeleteOldSessions(_maxSessionAge);
                var sessionsQuery = _plexSessionRepo.GetSessionsForEmail(loginResult.User.Email).Where(s => s.Token == user.Token && s.Username == user.Username);

                PlexAuthentication session;
                if (sessionsQuery.Any())
                {
                    session = sessionsQuery.Single(); // there should really be only one session at this point
                    // all we have to do now is update the time
                    session.SessionStart = DateTime.Now;
                    _plexSessionRepo.Update(session);
                }
                else
                {
                    // this sessions doesn't exist yet, so we'll save it now
                    session = _plexSessionRepo.Save(user.Email, user.Token, user.Username, DateTime.Now, user.Thumbnail);
                }

                return RedirectToAction("Index", new { Succes = true });
            }

            return RedirectToAction("Index", new { Succes = false, Error = loginResult.Error });
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
