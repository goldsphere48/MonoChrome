using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{

    [Flags]
    enum CacheMode
    {
        CacheOnAdd = 0,
        CacheOnEnable = 2,
        UnchacheOnRemove = 4,
        UncacheOnDisable = 16
    }

    class CacheRule 
    {
        public CacheMode CacheMode { get; }
        public CacheRule(CacheMode cacheMode)
        {
            CacheMode = cacheMode;
        }
    }

    class CacheItem<TKey>
    {
        public Component Component { get; }
        public TKey Key { get; }
        public CacheItem(Component component, TKey key)
        {
            Component = component;
            Key = key;
        }
    }

    abstract class CachedCollection<TKey, TCached> : ICachedCollection<TKey, TCached>
    {
        public abstract IEnumerable<TCached> this[TKey type] { get; }

        public void Register(GameObject gameObject)
        {
            RegisterHandlers(gameObject);
            var components = gameObject.GetComponents();
            foreach (var component in components)
            {
                if (component.Enabled)
                {
                    Cache(component, CacheMode.CacheOnAdd);
                }
            }
        }

        public void Erase(GameObject gameObject)
        {
            EraseHandlers(gameObject);
            foreach (var component in gameObject.GetComponents())
            {
                if (component.Enabled)
                {
                    Uncache(component, CacheMode.UnchacheOnRemove);
                }
            }
        }

        public abstract void AddCacheRule(CacheRule rule);
        public abstract void Clear();
        protected void RegisterHandlers(GameObject gameObject)
        {
            gameObject.ComponentAttached += OnComponentAdded;
            gameObject.ComponentDetach += OnComponentRemoved;
            gameObject.ComponentEnabled += OnComponentEnabled;
            gameObject.ComponentDisabled += OnComponentDisabled;
        }
        protected void EraseHandlers(GameObject gameObject)
        {
            gameObject.ComponentAttached += OnComponentAdded;
            gameObject.ComponentDetach += OnComponentRemoved;
            gameObject.ComponentEnabled += OnComponentEnabled;
            gameObject.ComponentDisabled += OnComponentDisabled;
        }
        protected void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheMode.CacheOnAdd);
        }
        protected void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheMode.UnchacheOnRemove);
        }
        protected void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheMode.CacheOnEnable);
        }
        protected void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheMode.UncacheOnDisable);
        }

        protected abstract void Cache(Component component, CacheMode rule);
        protected abstract void Uncache(Component component, CacheMode rule);
        protected abstract void Add(CacheItem<TKey> item);
        protected abstract bool Remove(CacheItem<TKey> item);
    }
}
