using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;

namespace PlexLander.Data
{
    public class AppRepository : IAppRepository
    {
        private PlexLanderContext _context;
        public AppRepository(PlexLanderContext context)
        {
            _context = context;
        }

        public void Add(App app)
        {
            _context.Add(app);
            _context.SaveChanges();
        }

        public IQueryable<App> ListAll()
        {
            return _context.Apps.AsNoTracking();
        }

        public void Remove(App app)
        {
            _context.Remove(app);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            App haveApp = _context.Apps.Where(a => a.Id == id).SingleOrDefault();
            if (haveApp != null)
            {
                Remove(haveApp);
            }
        }

        public void Update(App app)
        {
            _context.Update(app);
            _context.SaveChanges();
        }
    }
}
