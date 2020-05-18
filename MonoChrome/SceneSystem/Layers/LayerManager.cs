using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoChrome.Core;
using MonoChrome.SceneSystem.Input;
using System;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers
{
    public class LayerManager
    {
        private HashSet<FrameEndTask> _frameEndTasksBuffers = new HashSet<FrameEndTask>();
        private bool _isFrameEnd = true;
        private ILayerCollection _layers = new LayerStore();

        public Layer CreateLayer(string layerName, int zIndex)
        {
            if (string.IsNullOrEmpty(layerName) == false)
            {
                return _layers.CreateOrReplace(layerName, zIndex);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public ILayerSettings GetLayer(string layerName)
        {
            if (string.IsNullOrEmpty(layerName) == false)
            {
                return _layers.GetLayerSettings(layerName);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public ILayerSettings GetLayer(DefaultLayers layerName)
        {
            return GetLayer(layerName.ToString());
        }

        public void SetZIndex(string layerName, int zIndex)
        {
            if (string.IsNullOrEmpty(layerName) == false)
            {
                var layer = _layers.GetLayer(layerName);
                layer.ZIndex = zIndex;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void SetZIndex(DefaultLayers layerName, int zIndex)
        {
            SetZIndex(layerName.ToString(), zIndex);
        }

        internal void Add(string layerName, GameObject gameObject, bool replace = true)
        {
            if (string.IsNullOrEmpty(layerName) == false && gameObject != null)
            {
                if (string.IsNullOrEmpty(gameObject.LayerName))
                {
                    TryAddOnLayer(gameObject, layerName);
                } else if (gameObject.LayerName != layerName && replace)
                {
                    ReplaceGameObjectOnNewLayer(gameObject, layerName);
                }
            } else
            {
                throw new ArgumentNullException();
            }
        }

        internal void Add(DefaultLayers layerName, GameObject gameObject)
        {
            Add(layerName.ToString(), gameObject);
        }

        internal void Add(GameObject gameObject)
        {
            Add(DefaultLayers.Default.ToString(), gameObject);
        }

        internal void AddFrameEndTask(FrameEndTask task)
        {
            if (task != null)
            {
                if (_isFrameEnd)
                {
                    task?.Invoke();
                }
                else
                {
                    _frameEndTasksBuffers.Add(task);
                }
            } else
            {
                throw new ArgumentNullException();
            }
        }

        internal void Clear()
        {
            foreach (var layer in _layers)
            {
                layer.Clear();
            }
            _layers.Clear();
        }

        internal void Draw(SpriteBatch batch)
        {
            foreach (var layer in _layers)
            {
                layer.Draw(batch);
            }
        }

        internal void HandleMouseClick(PointerEventData pointerEventData)
        {
            foreach (var layer in _layers)
            {
                var clickWasHandled = layer.HandleMouseClick(pointerEventData);
                if (clickWasHandled && layer.AllowThroughHandling == false)
                {
                    break;
                }
            }
        }

        internal void HandleMouseMove(PointerEventData pointerEventData)
        {
            foreach (var layer in _layers)
            {
                var isMouseOver = layer.HandleMouseMove(pointerEventData);
                if (isMouseOver && layer.AllowThroughHandling == false)
                {
                    break;
                }
            }
        }

        internal void Initialize()
        {
            CreateLayer(DefaultLayers.Default, 0);
            var backgroundLayer = CreateLayer(DefaultLayers.Background, int.MinValue + 1000);
            var uiLayer = CreateLayer(DefaultLayers.UI, int.MaxValue - 100);
            var foregroundLayer = CreateLayer(DefaultLayers.Foreground, int.MaxValue - 1000);
            backgroundLayer.CollisionDetectionEnable = false;
            backgroundLayer.HandleInput = false;
            uiLayer.CollisionDetectionEnable = false;
            foregroundLayer.CollisionDetectionEnable = false;
            foregroundLayer.HandleInput = false;
        }

        internal void KeyboardHandle(KeyboardState state)
        {
            foreach (var layer in _layers)
            {
                layer.KeyboardHandle(state);
            }
        }

        internal void OnDestroy()
        {
            foreach (var layer in _layers)
            {
                layer.OnDestroy();
            }
        }

        internal void OnFinalise()
        {
            foreach (var layer in _layers)
            {
                layer.OnFinalise();
            }
        }

        internal void OnFrameBegin()
        {
            _isFrameEnd = false;
            foreach (var layer in _layers)
            {
                layer.OnFrameBegin();
            }
        }

        internal void OnFrameEnd()
        {
            _isFrameEnd = true;
            foreach (var layer in _layers)
            {
                layer.OnFrameEnd();
            }
            foreach (var task in _frameEndTasksBuffers)
            {
                task();
            }
            _frameEndTasksBuffers.Clear();
        }

        internal void Remove(GameObject gameObject)
        {
            if (gameObject != null && string.IsNullOrEmpty(gameObject.LayerName) == false)
            {
                string layerName = gameObject.LayerName;
                var layer = _layers.GetLayer(layerName);
                if (layer != null)
                {
                    layer.Remove(gameObject);
                    foreach (var child in gameObject.Transform.Childrens)
                    {
                        Remove(child.GameObject);
                    }
                }
                else
                {
                    throw new ArgumentException($"Can't find layer which contains gameObject {gameObject.Name}");
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        internal void Update()
        {
            foreach (var layer in _layers)
            {
                layer.Update();
            }
        }

        private void ReplaceGameObjectOnNewLayer(GameObject gameObject, string newLayerName)
        {
            Remove(gameObject);
            Add(newLayerName, gameObject);
        }

        private void TryAddOnLayer(GameObject gameObject, string layerName)
        {
            var layer = _layers.GetLayer(layerName);
            if (layer != null)
            {
                layer.Add(gameObject);
                foreach (var child in gameObject.Transform.Childrens)
                {
                    Add(layerName, child.GameObject, false);
                }
            }
            else
            {
                throw new ArgumentException($"Can't find layer with name {layerName}");
            }
        }

        private Layer CreateLayer(DefaultLayers layerName, int zIndex)
        {
            return CreateLayer(layerName.ToString(), zIndex);
        }
    }

    internal delegate void FrameEndTask();
}