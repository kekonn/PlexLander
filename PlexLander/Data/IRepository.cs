using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlexLander.Data
{
    public interface IRepository<TModel>
    {
        void Add(TModel model);
        void Update(TModel model);
        void Remove(TModel model);
        void Remove(int id);
    }
}
