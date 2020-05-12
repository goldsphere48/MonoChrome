using System;

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
