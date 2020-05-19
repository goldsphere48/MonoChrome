using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.SceneSystem.Input;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers
{
    public class Layer : ICollection<GameObject>, ILayerItem, ILayerSettings
    {
        public bool AllowThroughHandling { get; set; } = false;
        public bool CollisionDetectionEnable { get; set; } = true;
        public int Count => _gameObjects.Count;
        public bool Enabled { get; set; } = true;
        public bool HandleInput { get; set; } = true;
        public bool IsReadOnly => false;
        public string Name { get; }
        public bool Visible { get; set; } = true;
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                var oldValue = _zIndex;
                _zIndex = value;
                ZIndexChanged?.Invoke(this, new ZIndexEventArgs(oldValue));
            }
        }
        public event EventHandler<ZIndexEventArgs> ZIndexChanged;

        public Layer(string name, int zIndex)
        {
            Name = name;
            ZIndex = zIndex;
            var allCacheModes =
                CacheMode.CacheOnAdd |
                CacheMode.CacheOnEnable |
                CacheMode.UncacheOnDisable |
                CacheMode.UnchacheOnRemove;
            var onlyEntryCacheModes =
                CacheMode.CacheOnAdd |
                CacheMode.UnchacheOnRemove;
            _cachedComponents = new CachedComponents();
            _cachedMethods = new CachedMethods();
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(Renderer)));
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(Collider)));
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(IPointerClickHandler)));
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(IMouseOverHandler)));
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(IKeyboardHandler)));
            _cachedMethods.AddCacheRule(new MethodCacheRule(allCacheModes, "Update", component => component.UpdateMethod));
            _cachedMethods.AddCacheRule(new MethodCacheRule(onlyEntryCacheModes, "OnDestroy", component => component.OnDestroyMethod));
            _cachedMethods.AddCacheRule(new MethodCacheRule(onlyEntryCacheModes, "OnFinalize", component => component.OnFinalizeMethod));
        }

        private ICachedCollection<Type, Component> _cachedComponents;
        private ICachedCollection<string, Action> _cachedMethods;
        private Type _collider = typeof(Collider);
        private ICollection<GameObject> _gameObjects = new HashSet<GameObject>();
        private bool _isFrameEnd = false;
        private Type _keyboardHandler = typeof(IKeyboardHandler);
        private Type _mouseOverHandler = typeof(IMouseOverHandler);
        private HashSet<GameObject> _orderToAdd = new HashSet<GameObject>();
        private HashSet<GameObject> _orderToRemove = new HashSet<GameObject>();
        private Type _pointerClickHandler = typeof(IPointerClickHandler);
        private Type _renderer = typeof(Renderer);
        private int _zIndex;

        public void Add(GameObject item)
        {
            if (_isFrameEnd)
            {
                item.LayerName = Name;
                _cachedComponents.Register(item);
                _cachedMethods.Register(item);
                _gameObjects.Add(item);
            }
            else
            {
                _orderToAdd.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var gameObject in _gameObjects)
            {
                EraseItem(gameObject);
                gameObject.Dispose();
            }
            _cachedComponents.Clear();
            _cachedMethods.Clear();
            _gameObjects.Clear();
        }

        public int CompareTo(Layer other)
        {
            return other.ZIndex - ZIndex;
        }

        public bool Contains(GameObject item)
        {
            return _gameObjects.Contains(item);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _gameObjects.CopyTo(array, arrayIndex);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (Visible && Enabled)
            {
                foreach (Renderer renderer in _cachedComponents[_renderer])
                {
                    renderer.Draw(_spriteBatch);
                }
            }
        }

        public bool Equals(Layer other)
        {
            return base.Equals(other);
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HandleMouseClick(PointerEventData pointerEventData)
        {
            if (HandleInput)
            {
                return HandlePointerClick(pointerEventData);
            }
            return false;
        }

        public void OnDestroy()
        {
            foreach (var updateMethod in _cachedMethods["OnDestroy"])
            {
                updateMethod();
            }
        }

        public void OnFinalize()
        {
            foreach (var updateMethod in _cachedMethods["OnFinalize"])
            {
                updateMethod();
            }
        }

        public bool Remove(GameObject item)
        {
            if (_isFrameEnd)
            {
                EraseItem(item);
                GameObject.Erase(item);
                return _gameObjects.Remove(item);
            }
            else
            {
                _orderToRemove.Add(item);
                return true;
            }
        }

        public void Update()
        {
            if (Enabled)
            {
                foreach (var updateMethod in _cachedMethods["Update"])
                {
                    updateMethod();
                }
                if (CollisionDetectionEnable)
                {
                    CheckCollision();
                }
            }
        }

        internal bool HandleMouseMove(PointerEventData pointerEventData)
        {
            if (HandleInput)
            {
                return HandleMouseOver(pointerEventData);
            }
            return false;
        }

        internal void KeyboardHandle(KeyboardState state)
        {
            foreach (IKeyboardHandler component in _cachedComponents[_keyboardHandler])
            {
                component.KeyboardHandle(state);
            }
        }

        internal void OnFrameBegin()
        {
            _isFrameEnd = false;
            _cachedComponents.OnFrameBegin();
            _cachedMethods.OnFrameBegin();
        }

        internal void OnFrameEnd()
        {
            _isFrameEnd = true;
            _cachedComponents.OnFrameEnd();
            _cachedMethods.OnFrameEnd();
            foreach (var gameObject in _orderToAdd)
            {
                Add(gameObject);
            }
            foreach (var gameObject in _orderToRemove)
            {
                Remove(gameObject);
            }
            _orderToAdd.Clear();
            _orderToRemove.Clear();
        }

        private void CheckCollision()
        {
            foreach (Collider colliderA in _cachedComponents[_collider])
            {
                if (colliderA.CheckCollision)
                {
                    foreach (Collider colliderB in _cachedComponents[_collider])
                    {
                        if (colliderA != colliderB)
                        {
                            colliderA.CheckCollisionWith(colliderB);
                        }
                    }
                }
            }
        }

        private void EraseItem(GameObject item)
        {
            item.LayerName = null;
            _cachedComponents.Erase(item);
            _cachedMethods.Erase(item);
        }

        private bool HandleMouseOver(PointerEventData pointerEventData)
        {
            var handled = false;
            foreach (Component component in _cachedComponents[_mouseOverHandler])
            {
                var collider = component.GetComponent<Collider>(true);
                if (collider != null)
                {
                    var contains = collider.Contains(pointerEventData.Position);
                    if (contains)
                    {
                        (component as IMouseOverHandler).OnMouseOver();
                        collider.IsMouseOver = true;
                        handled = true;
                    }
                    else if (collider.IsMouseOver)
                    {
                        collider.IsMouseOver = false;
                        (component as IMouseOverHandler).OnMouseExit();
                    }
                }
            }
            return handled;
        }

        private bool HandlePointerClick(PointerEventData pointerEventData)
        {
            var clickWasHandled = false;
            foreach (Component component in _cachedComponents[_pointerClickHandler])
            {
                var collider = component.GetComponent<Collider>(true);
                if (collider != null)
                {
                    var contains = collider.Contains(pointerEventData.Position);
                    if (contains)
                    {
                        (component as IPointerClickHandler).OnPointerClick(pointerEventData);
                        clickWasHandled = true;
                    }
                }
            }
            return clickWasHandled;
        }
    }
}