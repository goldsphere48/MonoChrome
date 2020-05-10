using MonoChrome.Core;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers
{
    class LayerStore : ILayerCollection
    {
        private ZIndexSortedSet<Layer> _layers;

        public LayerStore()
        {
            _layers = new ZIndexSortedSet<Layer>();
        }

        public void Add(Layer layer)
        {
            if (!_layers.Contains(layer))
            {
                _layers.Add(layer);
            }
        }

        public void Clear()
        {
            _layers.Clear();
        }

        public bool Contains(string layerName)
        {
            foreach (var layer in _layers)
            {
                if (layer.Name == layerName)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<Layer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        public Layer GetLayer(string layerName)
        {
            foreach (var layer in _layers)
            {
                if (layer.Name == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        public bool Remove(Layer layer)
        {
            if (layer == null)
            {
                throw new ArgumentNullException();
            }
            return _layers.Remove(layer);
        }

        public bool Remove(string layerName)
        {
            var layer = GetLayer(layerName);
            return Remove(layer);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
