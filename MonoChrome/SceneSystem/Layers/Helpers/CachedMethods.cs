using MonoChrome.Core;
using System;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal class CachedMethods : CachedCollection<string, Action>
    {
        public override IEnumerable<Action> this[string type]
        {
            get
            {
                _cached.TryGetValue(type, out ZIndexSortedList<Component, Action> result);
                return result.Values;
            }
        }
        private IDictionary<string, ZIndexSortedList<Component, Action>> _cached = new Dictionary<string, ZIndexSortedList<Component, Action>>();
        private IDictionary<string, MethodReciver> _methodRecievers = new Dictionary<string, MethodReciver>();
        private IDictionary<string, CacheMode> _rules = new Dictionary<string, CacheMode>();

        public override void AddCacheRule(CacheRule cacheRule)
        {
            var rule = cacheRule as MethodCacheRule;
            var methodName = rule.MethodName;
            if (_cached.ContainsKey(methodName) == false)
            {
                _rules.Add(methodName, rule.CacheMode);
                _cached.Add(methodName, new ZIndexSortedList<Component, Action>());
                _methodRecievers.Add(methodName, rule.MethodReciever);
            }
        }

        public override void Clear()
        {
            _cached.Clear();
            _rules.Clear();
            _methodRecievers.Clear();
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
                _cached[item.Key].Add(item.Component, item.Action);
            }
        }

        protected override void Cache(Component component, CacheMode rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    SafeAdd(new MethodCacheItem(component, key, methodReciever.Value?.Invoke(component)));
                }
            }
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

        protected override void Uncache(Component component, CacheMode rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    SafeRemove(new MethodCacheItem(component, key, methodReciever.Value?.Invoke(component)));
                }
            }
        }
    }

    internal class MethodCacheItem : CacheItem<string>
    {
        public Action Action { get; }

        public MethodCacheItem(Component component, string key, Action action) : base(component, key)
        {
            Action = action;
        }
    }

    internal class MethodCacheRule : CacheRule
    {
        public string MethodName { get; }
        public MethodReciver MethodReciever { get; }

        public MethodCacheRule(CacheMode mode, string methodName, MethodReciver methodReciever) : base(mode)
        {
            MethodName = methodName;
            MethodReciever = methodReciever;
        }
    }

    internal delegate Action MethodReciver(Component component);
}