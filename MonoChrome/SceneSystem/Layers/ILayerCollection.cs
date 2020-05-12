using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers
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
