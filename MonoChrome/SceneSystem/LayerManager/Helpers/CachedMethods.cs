using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    delegate Action MethodReciver(Component component);
    
    class CachedMethods
    {

        private IDictionary<string, Action> _cached = new Dictionary<string, Action>();
        private IDictionary<string, CacheRule> _rules = new Dictionary<string, CacheRule>();
        private IDictionary<string, MethodReciver> _methodRecievers = new Dictionary<string, MethodReciver>();
        private EntityStore _store;

        public Action this[string type]
        {
            get
            {
                _cached.TryGetValue(type, out Action result);
                return result;
            }
        }

        public CachedMethods(EntityStore store)
        {
            _store = store;
            _store.ComponentAdded += OnComponentAdded;
            _store.ComponentRemoved += OnComponentRemoved;
            _store.ComponentEnabled += OnComponentEnabled;
            _store.ComponentDisabled += OnComponentDisabled;
        }

        public void AddCacheRule(string methodName, CacheRule rule, MethodReciver methodReciever)
        {
            if (!_cached.ContainsKey(methodName))
            {
                _rules.Add(methodName, rule);
                _cached.Add(methodName, null);
                _methodRecievers.Add(methodName, methodReciever);
            }
        }

        private void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheRule.CacheOnAdd);
        }
        private void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UnchacheOnRemove);
        }
        private void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheRule.CacheOnEnable);
        }
        private void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UncacheOnDisable);
        }

        private void Cache(Component component, CacheRule rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    Add(key, methodReciever.Value?.Invoke(component));
                }
            }
        }

        private void Uncache(Component component, CacheRule rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    Remove(key, methodReciever.Value?.Invoke(component));
                }
            }
        }

        public void Add(string key, Action action)
        {
            if (action == null)
            {
                return;
            }
            if (_cached.ContainsKey(key))
            {
                _cached[key] -= action;
                _cached[key] += action;
            } else
            {
                _cached[key] = action;
            }
        }

        public void Clear()
        {
            _cached.Clear();
            _rules.Clear();
            _methodRecievers.Clear();
        }

        public bool Remove(string key, Action action)
        {
            if (key != null && action != null)
            {
                _cached[key] -= action;
                return true;
            }
            return false;
        }
    }
}
