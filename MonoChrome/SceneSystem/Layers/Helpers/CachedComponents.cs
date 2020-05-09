using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class ComponentCacheRule : CacheRule
    {
        public Type ComponentType { get; }
        public ComponentCacheRule(CacheMode mode, Type componentType) : base(mode)
        {
            ComponentType = componentType;
        }
    }

    class ComponentCacheItem : CacheItem<Type>
    {
        public ComponentCacheItem(Component component, Type key) : base(component, key)
        {
        }
    }

    class CachedComponents : CachedCollection<Type, Component>
    {
        private IDictionary<Type, ICollection<Component>> _cached = new Dictionary<Type, ICollection<Component>>();
        private IDictionary<Type, CacheMode> _rules = new Dictionary<Type, CacheMode>();

        public override ICollection<Component> this[Type type]
        {
            get
            {
                _cached.TryGetValue(type, out ICollection<Component> result);
                if (result == null)
                {
                    return Enumerable.Empty<Component>() as IList<Component>;
                }
                return result;
            }
        }

        public override void AddCacheRule(CacheRule cacheRule)
        {
            var rule = cacheRule as ComponentCacheRule;
            if (!_cached.ContainsKey(rule.ComponentType))
            {
                _rules.Add(rule.ComponentType, rule.CacheMode);
                _cached.Add(rule.ComponentType, new SortedSet<Component>());
            }
        }

        public override void Clear()
        {
            _cached.Clear();
        }

        protected override void Cache(Component component, CacheMode rule)
        {
            var type = GetBaseType(component);
            if (type != null)
            {
                var cacheRule = _rules[type] & rule;
                if (cacheRule == rule)
                {
                    Add(new ComponentCacheItem(component, type));
                }
            }
        }

        protected override void Uncache(Component component, CacheMode rule)
        {
            var type = GetBaseType(component);
            if (type != null)
            {
                var cacheRule = _rules[type] & rule;
                if (cacheRule == rule)
                {
                    Remove(new ComponentCacheItem(component, type));
                }
            }
        }

        protected override void Add(CacheItem<Type> cacheItem)
        {
            var item = cacheItem as ComponentCacheItem;
            if (_cached.ContainsKey(item.Key))
            {
                var components = _cached[item.Key];
                if (!components.Contains(item.Component))
                {
                    Console.WriteLine(cacheItem.Key);
                    components.Add(item.Component);
                    item.Component.GameObject.ZIndexChanged += OnZIndexChanged;
                }
            }
        }

        protected override void OnZIndexChanged(object sender, EventArgs e)
        {
            var gameObject = sender as GameObject;
            var components = gameObject.GetComponents();
            foreach (var key in _cached.Keys)
            {
                foreach (var component in components)
                {
                    if (key.IsAssignableFrom(component.GetType()))
                    {
                        _cached[key].Remove(component);
                        _cached[key].Add(component);
                    }
                }
            }
        }

        protected override bool Remove(CacheItem<Type> cacheItem)
        {
            var item = cacheItem as ComponentCacheItem;
            if (_cached.ContainsKey(item.Key))
            {
                item.Component.GameObject.ZIndexChanged -= OnZIndexChanged;
                return _cached[item.Key].Remove(item.Component);
            }
            return false;
        }

        private Type GetBaseType(Component component)
        {
            var componentType = component.GetType();
            foreach (var key in _cached.Keys)
            {
                if (componentType.IsSubclassOf(key) || componentType == key)
                {
                    return key;
                }
            }
            return null;
        }
    }
}
