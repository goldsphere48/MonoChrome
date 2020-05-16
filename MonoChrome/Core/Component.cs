using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem;
using MonoChrome.SceneSystem.Layers;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
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
        public GraphicsDevice GraphicsDevice => Scene.GraphicsDevice;
        public bool IsDisposed => _disposed;
        public string LayerName => GameObject.LayerName;
        public Scene Scene => GameObject.Scene;
        public Transform Transform => GameObject.Transform;
        public int ZIndex
        {
            get => GameObject.ZIndex;
            set => GameObject.ZIndex = value;
        }
        internal bool IsAwaked { get; set; }
        internal bool IsStarted { get; set; }
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
        private bool _disposed = false;
        private bool _enabled = true;
        private EventHandler<ZIndexEventArgs> _zIndexChanged;

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

        public static void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        public static void Instatiate(GameObject gameObject)
        {
            GameObject.Instatiate(gameObject);
        }

        public static void Instatiate(GameObject gameObject, string layerName)
        {
            GameObject.Instatiate(gameObject, layerName);
        }

        public static void Instatiate(GameObject gameObject, DefaultLayers layer)
        {
            GameObject.Instatiate(gameObject, layer.ToString());
        }

        public void AddComponent(Type componentType)
        {
            GameObject.AddComponent(componentType);
        }

        public void AddComponent(Component newComponent)
        {
            GameObject.AddComponent(newComponent);
        }

        public void AddComponent<T>() where T : Component
        {
            GameObject.AddComponent<T>();
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

        public Component GetComponent(Type componentType, bool inherit = false)
        {
            return GameObject.GetComponent(componentType, inherit);
        }

        public T GetComponent<T>(bool inherit = false) where T : Component
        {
            return GameObject.GetComponent(typeof(T), inherit) as T;
        }

        public Component GetComponentInChildren(Type componentType, bool inherit = false)
        {
            return GameObject.GetComponentInChildren(componentType, inherit);
        }

        public T GetComponentInChildren<T>(bool inherit = false) where T : Component
        {
            return GameObject.GetComponentInChildren<T>(inherit);
        }

        public Component GetComponentInParent(Type componentType, bool inherit = false)
        {
            return GameObject.GetComponentInParent(componentType, inherit);
        }

        public T GetComponentInParent<T>(bool inherit = false) where T : Component
        {
            return GameObject.GetComponentInParent<T>(inherit);
        }

        public IEnumerable<Component> GetComponents()
        {
            return GameObject.GetComponents();
        }

        public IEnumerable<Component> GetComponentsInChildren(Type componentType, bool inherit = false)
        {
            return GameObject.GetComponentsInChildren(componentType, inherit);
        }

        public IEnumerable<T> GetComponentsInChildren<T>(bool inherit = false) where T : Component
        {
            return GameObject.GetComponentsInChildren<T>(inherit);
        }

        public IEnumerable<Component> GetComponentsInParent(Type componentType, bool inherit = false)
        {
            return GameObject.GetComponentsInParent(componentType, inherit);
        }

        public IEnumerable<T> GetComponentsInParent<T>(bool inherit = false) where T : Component
        {
            return GameObject.GetComponentsInParent<T>(inherit);
        }

        public bool HasComponent<T>(bool inherit = false) where T : Component
        {
            return GameObject.HasComponent<T>(inherit);
        }

        public void RemoveComponent(Type type)
        {
            GameObject.RemoveComponent(type);
        }

        public void RemoveComponent<T>() where T : Component
        {
            GameObject.RemoveComponent<T>();
        }

        public override string ToString()
        {
            return $"{GetType().Name} {GameObject}";
        }

        internal void OnZIndexChanged(object sender, ZIndexEventArgs e)
        {
            _zIndexChanged?.Invoke(this, e);
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
    }
}