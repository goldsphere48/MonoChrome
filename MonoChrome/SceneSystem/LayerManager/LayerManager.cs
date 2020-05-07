using Microsoft.Xna.Framework;
using MonoChrome.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.LayerManager
{
    class LayerManager
    {
        private ILayerCollection _layers = new LayerStore();

        public LayerManager()
        {
            CreateLayer(DefaultLayers.Default, 0);
            var backgroundLayer = CreateLayer(DefaultLayers.Background, int.MinValue + 1000);
            var uiLayer = CreateLayer(DefaultLayers.UI, int.MaxValue - 100);
            var foregroundLayer = CreateLayer(DefaultLayers.Foreground, int.MaxValue - 1000);
            backgroundLayer.CollisionDetectionEnable = false;
            backgroundLayer.HandleClickEnable = false;
            uiLayer.CollisionDetectionEnable = false;
            foregroundLayer.CollisionDetectionEnable = false;
            foregroundLayer.HandleClickEnable = false;
        }

        public void Add(string layerName, GameObject gameObject)
        {
            if (string.IsNullOrEmpty(layerName) || gameObject == null)
            {
                throw new ArgumentNullException();
            }
            var layer = _layers.GetLayer(layerName);
            if (layer != null)
            {
                layer.Add(gameObject);
            } else
            {
                throw new ArgumentException($"Can't find layer with name {layerName}");
            }
        }

        public void Add(DefaultLayers layerName, GameObject gameObject)
        {
            Add(layerName.ToString(), gameObject);
        }

        public void Add(GameObject gameObject)
        {
            Add(DefaultLayers.Default.ToString(), gameObject);
        }

        public bool Remove(GameObject gameObject)
        {
            string layerName = null;
            if (gameObject != null)
            {
                layerName = gameObject.LayerName;
            }
            if (string.IsNullOrEmpty(layerName))
            {
                throw new ArgumentNullException();
            }
            var layer = _layers.GetLayer(layerName);
            if (layer != null)
            {
                return layer.Remove(gameObject);
            }
            else
            {
                throw new ArgumentException($"Can't find layer which contains gameObject {gameObject.Name}");
            }
        }

        public bool Remove(DefaultLayers layerName, GameObject gameObject)
        {
            return Remove(gameObject);
        }

        public Layer CreateLayer(string layerName, int zIndex)
        {
            if (!string.IsNullOrEmpty(layerName))
            {
                var layer = _layers.GetLayer(layerName);
                if (layer == null)
                {
                    layer = new Layer(layerName, zIndex);
                    _layers.Add(layer);
                } else
                {
                    layer.ZIndex = zIndex;
                }
                return layer;
            } else
            {
                throw new ArgumentNullException();
            }
        }

        public void Clear()
        {
            foreach (var layer in _layers)
            {
                layer.Clear();
            }
            _layers.Clear();
        }

        private Layer CreateLayer(DefaultLayers layerName, int zIndex)
        {
            return CreateLayer(layerName.ToString(), zIndex);
        }
    }
}
