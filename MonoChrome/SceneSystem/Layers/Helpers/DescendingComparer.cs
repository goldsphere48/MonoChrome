using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal class DescendingComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }
}
