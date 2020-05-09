using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    abstract class CachedCollection<TKey, TCached>
    {
        public abstract ICollection<TCached> this[TKey type] { get; }

        public void Register(GameObject gameObject)
        {
            RegisterHandlers(gameObject);
            foreach (var component in gameObject.GetComponents())
            {
                if (component.Enabled)
                {
                    Cache(component, CacheRule.CacheOnAdd);
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
                    Uncache(component, CacheRule.UnchacheOnRemove);
                }
            }
        }

        public abstract void AddCacheRule<T>(CacheRule rule);
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
            Cache(componentArgs.Component, CacheRule.CacheOnAdd);
        }
        protected void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UnchacheOnRemove);
        }
        protected void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheRule.CacheOnEnable);
        }
        protected void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UncacheOnDisable);
        }

        protected abstract void Cache(Component component, CacheRule rule);
        protected abstract void Uncache(Component component, CacheRule rule);
        protected abstract void Add(Type key, Component component);
        protected abstract void OnZIndexChanged(object sender, EventArgs e);
        protected abstract bool Remove(Type key, Component component);

    }
}
