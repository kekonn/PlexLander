using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;
using PlexLander.Configuration;
using Microsoft.Extensions.Options;

namespace PlexLander.Data
{
    public class AppRepository : IAppRepository
    {
        private PlexLanderContext context;

        public AppRepository(PlexLanderContext context)
        {
            this.context = context;
        }

        public void Add(App app)
        {
            context.Add(app);
            context.SaveChanges();
        }

        public IQueryable<App> ListAll()
        {
            return context.Apps.AsNoTracking();
        }

        public void Remove(App app)
        {
            context.Remove(app);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            App haveApp = context.Apps.Where(a => a.Id == id).SingleOrDefault();
            if (haveApp != null)
            {
                Remove(haveApp);
            }
        }

        public void Update(App app)
        {
            context.Update(app);
            context.SaveChanges();
        }
    }
}
