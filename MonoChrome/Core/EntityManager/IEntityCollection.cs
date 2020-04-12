using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    interface IEntityCollection<TEntity> : ICollection<TEntity>
    {
        void Synchronize();
    }
}
