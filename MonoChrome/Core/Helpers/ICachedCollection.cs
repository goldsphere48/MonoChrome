using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers
{
    interface ICachedCollection<TKey, TCached>
    {
        void Cache(TKey key, TCached cached);
        bool Remove(TKey key);
        void Clear();
    }
}
