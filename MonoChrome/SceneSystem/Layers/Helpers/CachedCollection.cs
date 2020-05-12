using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    [Flags]
    internal enum CacheMode
    {
        CacheOnAdd = 0,
        CacheOnEnable = 2,
        UnchacheOnRemove = 4,
        UncacheOnDisable = 16
    }

    internal abstract class CachedCollection<TKey, TCached> : ICachedCollection<TKey, TCached>
    {
        public abstract IEnumerable<TCached> this[TKey type] { get; }
        public abstract void AddCacheRule(CacheRule rule);
        public abstract void Clear();
        public void Erase(GameObject gameObject)
        {
            EraseHandlers(gameObject);
            foreach (var component in gameObject.GetComponents())
            {
                Uncache(component, CacheMode.UnchacheOnRemove | CacheMode.UncacheOnDisable);
            }
        }
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
        protected abstract void Add(CacheItem<TKey> item);
        protected abstract void Cache(Component component, CacheMode rule);
        protected void EraseHandlers(GameObject gameObject)
        {
            gameObject.ComponentAttached -= OnComponentAdded;
            gameObject.ComponentDettach -= OnComponentRemoved;
            gameObject.ComponentEnabled -= OnComponentEnabled;
            gameObject.ComponentDisabled -= OnComponentDisabled;
        }
        protected void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheMode.CacheOnAdd);
        }
        protected void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheMode.UncacheOnDisable);
        }
        protected void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheMode.CacheOnEnable);
        }
        protected void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheMode.UnchacheOnRemove);
        }
        protected void RegisterHandlers(GameObject gameObject)
        {
            gameObject.ComponentAttached += OnComponentAdded;
            gameObject.ComponentDettach += OnComponentRemoved;
            gameObject.ComponentEnabled += OnComponentEnabled;
            gameObject.ComponentDisabled += OnComponentDisabled;
        }
        protected abstract bool Remove(CacheItem<TKey> item);
        protected abstract void Uncache(Component component, CacheMode rule);
    }

    internal class CacheItem<TKey>
    {
        public Component Component { get; }
        public TKey Key { get; }
        public CacheItem(Component component, TKey key)
        {
            Component = component;
            Key = key;
        }
    }

    internal class CacheRule
    {
        public CacheMode CacheMode { get; }
        public CacheRule(CacheMode cacheMode)
        {
            CacheMode = cacheMode;
        }
    }
}