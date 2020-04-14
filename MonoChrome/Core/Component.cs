using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    public abstract class Component : IDisposable
    {
        private bool _disposed = false;

        public static Component Create(Type componentType)
        {
            Component result = null;
            try
            {
                result = Activator.CreateInstance(componentType) as Component;
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException(string.Format(
                    "The component type '{0}' does not provide a parameter-less constructor.", componentType.ToString()));
            }
            return result;
        }

        public GameObject GameObject { get; private set; }

        private bool _enabled = true;
        public bool Enabled 
        {
            get => _enabled;
            set 
            {
                if (value)
                {
                    OnEnableMethod?.Invoke();
                    GameObject.Registry.OnComponentEnabled(this, GameObject);
                } else
                {
                    OnDisableMethod?.Invoke();
                    GameObject.Registry.OnComponentDisabled(this, GameObject);
                }
                _enabled = value;
            } 
        }

        internal Action AwakeMethod;
        internal Action UpdateMethod;
        internal Action OnEnableMethod;
        internal Action OnDisableMethod;
        internal Action OnFinaliseMethod;
        internal Action OnDestroyMethod;

        private Component()
        {
            AwakeMethod = CreateDelegate("Awake");
            UpdateMethod = CreateDelegate("Update");
            OnEnableMethod = CreateDelegate("OnEnable");
            OnDisableMethod = CreateDelegate("OnDisable");
            OnFinaliseMethod = CreateDelegate("OnFinalise");
            OnDestroyMethod = CreateDelegate("OnDestroy");
        }

        internal void Attach(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        internal void Dettach()
        {
            GameObject = null;
        }

        private Action CreateDelegate(string name)
        {
            return Delegate.CreateDelegate(typeof(void), GetMethod(name)) as Action;
        }

        private MethodInfo GetMethod(string name)
        {
            return GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool clean)
        {
            if (!_disposed)
            {
                if (clean)
                {
                    OnDestroyMethod?.Invoke();
                }
                OnFinaliseMethod?.Invoke();
            }
        }

        ~Component()
        {
            Dispose(false);
        }
    }
}
