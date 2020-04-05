using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    public abstract class Playable
    {
        private bool _enabled = true;
        public bool Enabled 
        { 
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }
        public abstract void Awake();
        public abstract void Start();
        public abstract void Update();
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnDestroy();
        public abstract void OnFinalize();
    }
}
