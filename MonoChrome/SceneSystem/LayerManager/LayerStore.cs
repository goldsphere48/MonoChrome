﻿using MonoChrome.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.LayerManager
{
    class LayerComparer : IComparer<Layer>
    {
        public int Compare(Layer x, Layer y)
        {
            return x.ZIndex - y.ZIndex;
        }
    }

    class LayerStore : ILayerCollection
    {
        private SortedSet<Layer> _layers;

        public LayerStore()
        {
            _layers = new SortedSet<Layer>(new LayerComparer());
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
