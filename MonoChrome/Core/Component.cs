using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Reflection;

namespace MonoChrome.Core
{
    public abstract class Component : IDisposable, ILayerItem
    {
        public ContentManager Content => Scene.Content;
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
                }
                else
                {
                    OnDisableMethod?.Invoke();
                    ComponentDisabled?.Invoke(this, new ComponentEventArgs(this, GameObject));
                }
                _enabled = value;
            }
        }
        public GameObject GameObject { get; internal set; }
        public bool IsDisposed => _disposed;
        public GraphicsDevice GraphicsDevice => Scene.GraphicsDevice;
        public string LayerName => GameObject.LayerName;
        public Scene Scene => GameObject.Scene;
        public Transform Transform => GameObject.Transform;
        public int ZIndex
        {
            get => GameObject.ZIndex;
            set => GameObject.ZIndex = value;
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

        public override string ToString()
        {
            return $"{GetType().Name} {GameObject}";
        }
        internal bool IsAwaked { get; set; }
        internal bool IsStarted { get; set; }
        internal Action AwakeMethod;
        internal ComponentEventHandler ComponentDisabled;
        internal ComponentEventHandler ComponentEnabled;
        internal Action<Collision> OnCollision;
        internal Action OnDestroyMethod;
        internal Action OnDisableMethod;
        internal Action OnEnableMethod;
        internal Action OnFinaliseMethod;
        internal Action StartMethod;
        internal Action UpdateMethod;
        internal void OnZIndexChanged(object sender, ZIndexEventArgs e)
        {
            _zIndexChanged?.Invoke(this, e);
        }
        protected Component()
        {
            AwakeMethod = CreateDelegate("Awake");
            StartMethod = CreateDelegate("Start");
            UpdateMethod = CreateDelegate("Update");
            OnEnableMethod = CreateDelegate("OnEnable");
            OnDisableMethod = CreateDelegate("OnDisable");
            OnFinaliseMethod = CreateDelegate("OnFinalise");
            OnDestroyMethod = CreateDelegate("OnDestroy");
            OnCollision = CreateDelegate<Collision>("OnCollision");
        }
        ~Component()
        {
            Dispose(false);
        }
        private bool _disposed = false;
        private bool _enabled = true;
        private EventHandler<ZIndexEventArgs> _zIndexChanged;
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
    }
}