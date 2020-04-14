﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class CachedMethods : ICachedCollection<string, Action>
    {
        private IDictionary<string, Action> _cached = new Dictionary<string, Action>();
        public Action this[string type]
        {
            get
            {
                _cached.TryGetValue(type, out Action result);
                return result;
            }
        }

        public void Cache(string key, Action action)
        {
            if (_cached.ContainsKey(key))
            {
                _cached[key] += action;
            } else
            {
                _cached[key] = action;
            }
        }

        public void Clear()
        {
            _cached.Clear();
        }

        public bool Remove(string key)
        {
            if (key != null)
            {
                return _cached.Remove(key);
            }
            return false;
        }
    }
}
