using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    [Flags]
    enum CacheRule
    {
        CacheOnAdd = 0,
        CacheOnEnable = 2,
        UnchacheOnRemove = 4,
        UncacheOnDisable = 16
    }
}
