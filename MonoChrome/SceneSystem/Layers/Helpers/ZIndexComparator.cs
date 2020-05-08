using MonoChrome.Core;
using MonoChrome.SceneSystem.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class ZIndexComparator : IComparer<IZIndex>
    {
        public int Compare(IZIndex x, IZIndex y)
        {
            return y.ZIndex - x.ZIndex;
        }
    }
}
