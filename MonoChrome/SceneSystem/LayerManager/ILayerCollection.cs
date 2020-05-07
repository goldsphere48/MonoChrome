using MonoChrome.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.LayerManager
{
    interface ILayerCollection : IEnumerable<Layer>
    {
        bool Contains(string layerName);
        void Add(Layer layer);
        bool Remove(Layer layer);
        bool Remove(string layerName);
        Layer GetLayer(string layerName);
        void Clear();
    }
}
