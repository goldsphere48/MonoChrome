using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.Core.Helpers;
using MonoChrome.SceneSystem.Input;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers
{
    class Layer : ICollection<GameObject>, IZIndex
    {
        public string Name { get; }
        public int Count => _gameObjects.Count;
        public bool IsReadOnly => false;
        public bool CollisionDetectionEnable { get; set; } = true;
        public bool HandleClickEnable { get; set; } = true;
        public bool AllowThroughHandling { get; set; } = false;
        public object CachedRule { get; }
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                ZIndexChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> ZIndexChanged;

        private ICollection<GameObject> _gameObjects = new HashSet<GameObject>();
        private ICachedCollection<Type, Component> _cachedComponents;
        private ICachedCollection<string, Action> _cachedMethods;
        private Type _renderer = typeof(Renderer);
        private Type _collider = typeof(Collider);
        private Type _mouseClickHandler = typeof(IMouseClickHandler);
        private Type _pointerClickHandler = typeof(IPointerClickHandler);
        private int _zIndex;

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
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(IMouseClickHandler)));
            _cachedComponents.AddCacheRule(new ComponentCacheRule(allCacheModes, typeof(IPointerClickHandler)));
            _cachedMethods.AddCacheRule(new MethodCacheRule(allCacheModes, "Update", component => component.UpdateMethod));
            _cachedMethods.AddCacheRule(new MethodCacheRule(onlyEntryCacheModes, "OnDestroy", component => component.OnDestroyMethod));
            _cachedMethods.AddCacheRule(new MethodCacheRule(onlyEntryCacheModes, "OnFinalise", component => component.OnFinaliseMethod));
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (SpriteRenderer renderer in _cachedComponents[_renderer])
            {
                renderer.Draw(_spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var updateMethod in _cachedMethods["Update"])
            {
                updateMethod();
            }
            if (CollisionDetectionEnable)
            {
                foreach (Collider colliderA in _cachedComponents[_collider])
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

        public void OnDestroy()
        {
            foreach (var updateMethod in _cachedMethods["OnDestroy"])
            {
                updateMethod();
            }
        }

        public void OnFinalise()
        {
            foreach (var updateMethod in _cachedMethods["OnFinalise"])
            {
                updateMethod();
            }
        }

        public void Add(GameObject item)
        {
            item.LayerName = Name;
            _cachedComponents.Register(item);
            _cachedMethods.Register(item);
            _gameObjects.Add(item);
        }

        public void Clear()
        {
            foreach (var gameObject in _gameObjects)
            {
                Remove(gameObject);
            }
            _cachedComponents.Clear();
            _cachedMethods.Clear();
            _gameObjects.Clear();
        }

        public bool Contains(GameObject item)
        {
            return _gameObjects.Contains(item);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _gameObjects.CopyTo(array, arrayIndex);
        }

        public bool Remove(GameObject item)
        {
            item.LayerName = null;
            _cachedComponents.Erase(item);
            _cachedMethods.Erase(item);
            return _gameObjects.Remove(item);
        }

        public bool HandleMouseClick(PointerEventData pointerEventData)
        {
            if (HandleClickEnable)
            {
                var clickWasHandled = HandleSampleMouseClick(pointerEventData);
                clickWasHandled = HandlePointerClick(pointerEventData) || clickWasHandled;
                return clickWasHandled;
            }
            return false;
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool HandleSampleMouseClick(PointerEventData pointerEventData)
        {
            var clickWasHandled = false;
            foreach (IMouseClickHandler mouseClickHandler in _cachedComponents[_mouseClickHandler])
            {
                mouseClickHandler.OnMouseClick(pointerEventData);
                clickWasHandled = true;
            }
            return clickWasHandled;
        }

        private bool HandlePointerClick(PointerEventData pointerEventData)
        {
            var clickWasHandled = false;
            foreach (Component component in _cachedComponents[_pointerClickHandler])
            {
                var collider = component.GetComponent<Collider>();
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
