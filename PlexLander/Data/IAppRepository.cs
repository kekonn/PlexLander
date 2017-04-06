using PlexLander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Data
{
    public interface IAppRepository
    {
        IQueryable<App> ListAll();
        void Add(App app);
        void Update(App app);
        void Remove(App app);
        void Remove(int id);
    }
}
