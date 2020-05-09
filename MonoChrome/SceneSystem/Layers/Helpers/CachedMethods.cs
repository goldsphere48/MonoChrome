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
    
    class CachedMethods : CachedCollection<string, Action>
    {
        private static ZIndexComparator componentZIndexComporator = new ZIndexComparator();
        private IDictionary<string, IDictionary<Component, Action>> _cached = new Dictionary<string, IDictionary<Component, Action>>();
        private IDictionary<string, CacheRule> _rules = new Dictionary<string, CacheRule>();
        private IDictionary<string, MethodReciver> _methodRecievers = new Dictionary<string, MethodReciver>();

        public override ICollection<Action> this[string type]
        {
            get
            {
                _cached.TryGetValue(type, out IDictionary<Component, Action> result);
                return result.Values;
            }
        }

        public void AddCacheRule(string methodName, CacheRule rule, MethodReciver methodReciever)
        {
            if (!_cached.ContainsKey(methodName))
            {
                _rules.Add(methodName, rule);
                _cached.Add(methodName, new SortedList<Component, Action>(componentZIndexComporator));
                _methodRecievers.Add(methodName, methodReciever);
            }
        }

        public override void Add(string key, Action action, Component component)
        {
            if (action == null)
            {
                return;
            }
            if (_cached.ContainsKey(key))
            {
                _cached[key].Add(component, action);
                component.ZIndexChanged += OnZIndexChanged;
            }
        }

        public override void Clear()
        {
            _cached.Clear();
            _rules.Clear();
            _methodRecievers.Clear();
        }

        public override bool Remove(string key, Action action, Component component)
        {
            if (key != null && action != null && component != null)
            {
                return _cached[key].Remove(component);
            }
            return false;
        }

        protected override void Cache(Component component, CacheRule rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    //Console.WriteLine("In Cache " + key);
                    Add(key, methodReciever.Value?.Invoke(component), component);
                }
            }
        }

        protected override void Uncache(Component component, CacheRule rule)
        {
            foreach (var methodReciever in _methodRecievers)
            {
                var key = methodReciever.Key;
                var cacheRule = _rules[key] & rule;
                if (cacheRule == rule)
                {
                    Remove(key, methodReciever.Value?.Invoke(component), component);
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
