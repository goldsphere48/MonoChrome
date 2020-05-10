using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    public class ZIndexEventArgs : EventArgs
    {
        public int OldZIndex { get; set; }

        public ZIndexEventArgs(int oldZIndex)
        {
            OldZIndex = oldZIndex;
        }
    }
}
