using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers
{
    internal interface ILayerCollection : IEnumerable<Layer>
    {
        void Add(Layer layer);
        void Clear();
        bool Contains(string layerName);
        Layer GetLayer(string layerName);
        bool Remove(Layer layer);
        bool Remove(string layerName);
    }
}