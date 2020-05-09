using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    delegate Action MethodReciver(Component component);

    class MethodCacheRule : CacheRule
    {
        public string MethodName { get; }
        public MethodReciver MethodReciever { get; }
        public MethodCacheRule(CacheMode mode, string methodName, MethodReciver methodReciever) : base(mode)
        {
            MethodName = methodName;
            MethodReciever = methodReciever;
        }
    }

    class MethodCacheItem : CacheItem<string>
    {
        public Action Action { get; }
        public MethodCacheItem(Component component, string key, Action action) : base(component, key)
        {
            Action = action;
        }
    }

    class CachedMethods : CachedCollection<string, Action>
    {
        private IDictionary<string, IDictionary<Component, Action>> _cached = new Dictionary<string, IDictionary<Component, Action>>();
        private IDictionary<string, CacheMode> _rules = new Dictionary<string, CacheMode>();
        private IDictionary<string, MethodReciver> _methodRecievers = new Dictionary<string, MethodReciver>();

        public override ICollection<Action> this[string type]
        {
            get
            {
                _cached.TryGetValue(type, out IDictionary<Component, Action> result);
                return result.Values;
            }
        }

        public override void AddCacheRule(CacheRule cacheRule)
        {
            var rule = cacheRule as MethodCacheRule;
            var methodName = rule.MethodName;
            if (!_cached.ContainsKey(methodName))
            {
                _rules.Add(methodName, rule.CacheMode);
                _cached.Add(methodName, new SortedList<Component, Action>());
                _methodRecievers.Add(methodName, rule.MethodReciever);
            }
        }

        protected override void Add(CacheItem<string> cacheItem)
        {
            var item = cacheItem as MethodCacheItem;
            if (item.Action == null)
            {
                return;
            }
            if (_cached.ContainsKey(item.Key))
            {
                Console.WriteLine("Method " + item.Key);
                _cached[item.Key].Add(item.Component, item.Action);
                item.Component.ZIndexChanged += OnZIndexChanged;
            }
        }

        public override void Clear()
        {
            _cached.Clear();
            _rules.Clear();
            _methodRecievers.Clear();
        }

        protected override bool Remove(CacheItem<string> cacheItem)
        {
            var item = cacheItem as MethodCacheItem;
            if (item.Key != null && item.Action != null && item.Component != null)
            {
                return _cached[item.Key].Remove(item.Component);
            }
            return false;
        }

        protected override void Cache(Component component, CacheMode rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    Add(new MethodCacheItem(component, key, methodReciever.Value?.Invoke(component)));
                }
            }
        }

        protected override void Uncache(Component component, CacheMode rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    Remove(new MethodCacheItem(component, key, methodReciever.Value?.Invoke(component)));
                }
            }
        }

        protected override void OnZIndexChanged(object sender, EventArgs e)
        {
            var gameObject = sender as GameObject;
            var components = gameObject.GetComponents();
            foreach (var value in _cached.Values)
            {
                foreach (var component in components)
                {
                    if (value.Keys.Contains(component))
                    {
                        var values = value[component];
                        value.Remove(component);
                        value.Add(component, values);
                    }
                }
            }
        }
    }
}
