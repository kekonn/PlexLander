using PlexLander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Data
{
    public interface IAppRepository : IRepository<App>
    {
        IQueryable<App> ListAll();
    }
}
