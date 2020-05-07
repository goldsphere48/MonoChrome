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
    class Layer : ICollection<GameObject>
    {
        public string Name { get; }
        public int ZIndex { get; set; }
        public int Count => _gameObjects.Count;
        public bool IsReadOnly => false;
        public bool CollisionDetectionEnable { get; set; } = true;
        public bool HandleClickEnable { get; set; } = true;
        public bool AllowThroughHandling { get; set; } = false;
        public object CachedRule { get; }

        private ICollection<GameObject> _gameObjects = new HashSet<GameObject>();
        private CachedComponents _cachedComponents;
        private CachedMethods _cachedMethods;
        private Type _renderer = typeof(Renderer);
        private Type _collider = typeof(Collider);
        private Type _mouseClickHandler = typeof(IMouseClickHandler);
        private Type _pointerClickHandler = typeof(IPointerClickHandler);

        public Layer(string name, int zIndex, EntityStore store)
        {
            Name = name;
            ZIndex = zIndex;
            var allCahceRules = CacheRule.CacheOnAdd | 
                                CacheRule.CacheOnEnable |
                                CacheRule.UncacheOnDisable | 
                                CacheRule.UnchacheOnRemove;
            _cachedComponents = new CachedComponents(store);
            _cachedMethods = new CachedMethods(store);
            _cachedComponents.AddCacheRule<Renderer>(allCahceRules);
            _cachedComponents.AddCacheRule<Collider>(allCahceRules);
            _cachedComponents.AddCacheRule<IMouseClickHandler>(allCahceRules);
            _cachedComponents.AddCacheRule<IPointerClickHandler>(allCahceRules);
            _cachedMethods.AddCacheRule("Update", allCahceRules, component => component.UpdateMethod);
            _cachedMethods.AddCacheRule("OnDestroy", CacheRule.CacheOnAdd | CacheRule.UnchacheOnRemove, component => component.OnDestroyMethod);
            _cachedMethods.AddCacheRule("OnFinalise", CacheRule.CacheOnAdd | CacheRule.UnchacheOnRemove, component => component.OnFinaliseMethod);
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
            _cachedMethods["Update"]?.Invoke();
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
            _cachedMethods["OnDestroy"]?.Invoke();
        }

        public void OnFinalise()
        {
            _cachedMethods["OnFinalise"]?.Invoke();
        }

        public void Add(GameObject item)
        {
            item.LayerName = Name;
            _gameObjects.Add(item);
        }

        public void Clear()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.LayerName = null;
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
