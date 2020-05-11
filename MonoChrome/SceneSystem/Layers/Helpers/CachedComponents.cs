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

        public override IEnumerable<Component> this[Type type]
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
                _cached.Add(rule.ComponentType, new ZIndexSortedSet<Component>());
            }
        }

        public override void Clear()
        {
            _cached.Clear();
        }

        protected override void Cache(Component component, CacheMode rule)
        {
            var types = GetBaseType(component);
            foreach (var type in types)
            {
                if (type != null)
                {
                    var cacheRule = _rules[type] & rule;
                    if (cacheRule == rule)
                    {
                        Add(new ComponentCacheItem(component, type));
                    }
                }
            }
        }

        protected override void Uncache(Component component, CacheMode rule)
        {
            var types = GetBaseType(component);
            foreach (var type in types)
            {
                if (type != null)
                {
                    var cacheRule = _rules[type] & rule;
                    if (cacheRule == rule)
                    {
                        Remove(new ComponentCacheItem(component, type));
                    }
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
                    components.Add(item.Component);
                }
            }
        }

        protected override bool Remove(CacheItem<Type> cacheItem)
        {
            var item = cacheItem as ComponentCacheItem;
            if (_cached.ContainsKey(item.Key))
            {
                return _cached[item.Key].Remove(item.Component);
            }
            return false;
        }

        private IEnumerable<Type> GetBaseType(Component component)
        {
            var componentType = component.GetType();
            foreach (var key in _cached.Keys)
            {
                if (componentType.IsSubclassOf(key) || componentType == key)
                {
                    yield return key;
                }
                if (key.IsAssignableFrom(componentType))
                {
                    yield return key;
                }
            }
        }
    }
}
