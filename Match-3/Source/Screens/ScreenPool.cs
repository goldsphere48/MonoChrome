using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    class ScreenPool
    {
        private Dictionary<string, IScreenController> _screens = new Dictionary<string, IScreenController>();

        public void Add(string name, IScreenController screen)
        {
            if (Contains(name))
            {
                throw new Exception($"Screen {name} already exist");
            }
            else
            {
                _screens.Add(name, screen);
            }
        }

        public void Remove(string name)
        {
            if (Contains(name))
            {
                throw new Exception($"Screen {name} already exist");
            }
            else
            {
                _screens.Remove(name);
            }
        }

        public IScreenController Get(string name)
        {
            if (Contains(name))
            {
                return _screens[name];
            } 
            else
            {
                throw new Exception($"Screen {name} not found");    
            }
        }

        private bool Contains(string key)
        {
            return _screens.Keys.Contains(key);
        }
    }
}
