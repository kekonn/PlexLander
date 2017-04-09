using PlexLander.Configuration;
using PlexLander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.ViewModels
{
    public class AppViewModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Endpoint { get; set; }
        public string Token { get; set; }
    }

    public static class AppViewModelFactory
    {
        public static AppViewModel FromBuiltInApp(BuiltInApp app, int? Id)
        {
            var appVM =  new AppViewModel
            {
                Name = app.Name,
                Icon = app.Icon,
                Endpoint = app.Endpoint,
                Url = app.Url,
                Token = app.Token
            };
            if (Id.HasValue)
            {
                appVM.Id = Id.Value;
            }

            return appVM;
        }

        public static AppViewModel FromUserApp(App app)
        {
            return new AppViewModel
            {
                Id = app.Id,
                Name = app.Name,
                Icon = app.Icon,
                Url = app.Url
            };
        }

        public static IEnumerable<AppViewModel> FromUserApps(IEnumerable<App> apps)
        {
            return apps.Select(a => FromUserApp(a));
        }

        public static IEnumerable<AppViewModel> FromBuiltInApps(IEnumerable<BuiltInApp> apps)
        {
            return apps.Select(a => FromBuiltInApp(a, null));
        }

        public static IEnumerable<AppViewModel> FromBuiltInApps(IDictionary<BuiltInApp,int?> apps)
        {
            return apps.Select(ad => FromBuiltInApp(ad.Key, ad.Value));
        }
        
        public static IEnumerable<AppViewModel> FromApps(IEnumerable<BuiltInApp> builtInApps, IEnumerable<App> userApps)
        {
            var appList = new List<AppViewModel>(builtInApps.Count() + userApps.Count());
            appList.AddRange(FromBuiltInApps(builtInApps));
            appList.AddRange(FromUserApps(userApps));
            return appList.AsEnumerable();
        }
    }
}
