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

        public async void Remove(App app)
        {
            _context.Remove(app);
            await _context.SaveChangesAsync();
        }

        public async void Remove(int id)
        {
            App haveApp = await _context.Apps.Where(a => a.Id == id).SingleOrDefaultAsync();
            if (haveApp != null)
            {
                Remove(haveApp);
            }
        }

        public async void Update(App app)
        {
            _context.Update(app);
            await _context.SaveChangesAsync();
        }
    }
}
