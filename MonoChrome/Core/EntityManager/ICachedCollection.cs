using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    interface ICachedCollection<TCached, TKey>
    {
        IList<TCached> this[TKey type] { get; }
        void Cache(TCached cached);
        bool Remove(TCached cached);
        void Clear();
    }
}
