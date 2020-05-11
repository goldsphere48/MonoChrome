using Microsoft.Xna.Framework;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem;
using MonoChrome.SceneSystem.Input;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    public abstract class Component : IDisposable, ILayerItem
    {
        public static Component Create(Type componentType)
        {
            Component result;
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

        public event EventHandler<ZIndexEventArgs> ZIndexChanged
        {
            add
            {
                _zIndexChanged -= value;
                _zIndexChanged += value;
            }
            remove
            {
                _zIndexChanged -= value;
            }
        }

        public GameObject GameObject { get; internal set; }
        public Transform Transform => GameObject.Transform;
        public Scene Scene => GameObject.Scene;

        public bool Enabled 
        {
            get => _enabled;
            set 
            {
                if (value == _enabled)
                {
                    return;
                }
                if (value)
                {
                    OnEnableMethod?.Invoke();
                    ComponentEnabled?.Invoke(this, new ComponentEventArgs(this, GameObject));
                } else
                {
                    OnDisableMethod?.Invoke();
                    ComponentEnabled?.Invoke(this, new ComponentEventArgs(this, GameObject));
                }
                _enabled = value;
            } 
        }

        public int ZIndex 
        { 
            get => GameObject.ZIndex;
            set => GameObject.ZIndex = value;
        }

        public string LayerName => GameObject.LayerName;

        internal ComponentEventHandler ComponentEnabled;
        internal ComponentEventHandler ComponentDisabled;

        internal Action AwakeMethod;
        internal Action UpdateMethod;
        internal Action OnEnableMethod;
        internal Action OnDisableMethod;
        internal Action OnFinaliseMethod;
        internal Action OnDestroyMethod;
        internal Action<Collision> OnCollision;

        private bool _enabled = true;
        private bool _disposed = false;
        private EventHandler<ZIndexEventArgs> _zIndexChanged;

        protected Component()
        {
            AwakeMethod = CreateDelegate("Awake");
            UpdateMethod = CreateDelegate("Update");
            OnEnableMethod = CreateDelegate("OnEnable");
            OnDisableMethod = CreateDelegate("OnDisable");
            OnFinaliseMethod = CreateDelegate("OnFinalise");
            OnDestroyMethod = CreateDelegate("OnDestroy");
            OnCollision = CreateDelegate<Collision>("OnCollision");
        }

        private Action CreateDelegate(string name)
        {
            return GetMethod(name)?.CreateDelegate(typeof(Action), this) as Action;
        }

        private Action<T> CreateDelegate<T>(string name)
        {
            return GetMethod(name)?.CreateDelegate(typeof(Action<T>), this) as Action<T>;
        }

        private MethodInfo GetMethod(string name)
        {
            return GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
                if (_enabled)
                {
                    OnDisableMethod?.Invoke();
                }
                if (clean)
                {
                    OnDestroyMethod?.Invoke();
                }
                OnFinaliseMethod?.Invoke();
                _disposed = true;
            }
        }

        ~Component()
        {
            Dispose(false);
        }

        internal void OnZIndexChanged(object sender, ZIndexEventArgs e)
        {
            _zIndexChanged?.Invoke(this, e);
        }
    }
}
