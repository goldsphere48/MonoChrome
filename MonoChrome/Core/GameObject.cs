using MonoChrome.Core.Components;
using MonoChrome.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Input;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.SceneSystem.Layers.Helpers;
using MonoChrome.SceneSystem;

namespace MonoChrome.Core
{
    class ComponentAttachEventArgs : EventArgs
    {
        public Component Component { get; set; }
        public ComponentAttachEventArgs(Component component)
        {
            Component = component;
        }

    }

    public sealed class GameObject : IDisposable, ILayerItem
    {
        public const string DefaultName = "GameObject";
        public string Name { get; }
        public string LayerName { get; internal set; }
        public Transform Transform { get => GetComponent<Transform>(); }
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
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                var components = Registry.GetComponents(this);
                foreach (var component in components)
                {
                    component.Enabled = _enabled;
                }
                var transform = Transform;
                foreach (var child in transform.Childrens)
                {
                    child.GameObject.Enabled = _enabled;
                }
            }
        }

        public Scene Scene
        {
            get => _scene;
            internal set
            {
                _scene = value;
                foreach (var child in Transform.Childrens)
                {
                    child.GameObject.Scene = _scene;
                }
            }
        }

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                var oldValue = _zIndex;
                _zIndex = value;
                _zIndexChanged?.Invoke(this, new ZIndexEventArgs(oldValue));
            }
        }

        internal event ComponentEventHandler ComponentAttached;
        internal event ComponentEventHandler ComponentDetach;
        internal event ComponentEventHandler ComponentEnabled;
        internal event ComponentEventHandler ComponentDisabled;
        internal EntityStore Registry { get; set; }

        private Scene _scene;
        private EventHandler<ZIndexEventArgs> _zIndexChanged;
        private int _zIndex;
        private bool _enabled = true;


        internal GameObject(string name, EntityStore store)
        {
            Name = name;
            Registry = store;
        }

        #region Components Controller
        public void AddComponent(Type componentType)
        {
            var component = Component.Create(componentType);
            Registry.Add(this, component);
        }

        public void AddComponent(Component component)
        {
            Registry.Add(this, component);
        }

        public void AddComponent<T>() where T : Component
        {
            AddComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            var removableComponent = GetComponent(type);
            if (removableComponent != null)
            {
                Registry.Remove(this, removableComponent);
            }
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public bool HasComponent<T>(bool inherit = false) where T : Component
        {
            return Registry.HasComponent<T>(this, inherit);
        }

        public Component GetComponent(Type componentType, bool inherit = false)
        {
            return Registry.GetComponent(this, componentType, inherit);
        }
        public T GetComponent<T>(bool inherit = false) where T : Component
        {
            return GetComponent(typeof(T), inherit) as T;
        }

        public IEnumerable<Component> GetComponents(Type componentType, bool inherit = true)
        {
            return Registry.GetComponents(this, componentType, inherit);
        }
        public IEnumerable<T> GetComponents<T>(bool inherit = true) where T : Component
        {
            return GetComponents(typeof(T), inherit) as IEnumerable<T>;
        }

        public Component GetComponentInChildren(Type componentType, bool inherit = false)
        {
            var component = GetComponent(componentType, inherit);
            var transform = Transform;
            if (component == null)
            {
                for (int i = 0; i < transform.Childrens.Count; ++i)
                {
                    component = transform.Childrens[i].GameObject.GetComponentInChildren(componentType);
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return component;
        }

        public T GetComponentInChildren<T>(bool inherit = false) where T : Component
        {
            return GetComponentInChildren(typeof(T), inherit) as T;
        }

        public IEnumerable<Component> GetComponentsInChildren(Type componentType, bool inherit = false)
        {
            var currentComponents = GetComponents(componentType, inherit);
            var transform = Transform;
            if (currentComponents != null)
            {
                foreach (var component in currentComponents)
                {
                    yield return component;
                }
            }
            foreach (var child in transform.Childrens)
            {
                var childrenComponents = child.GameObject.GetComponentsInChildren(componentType, inherit);
                foreach (var childrenComponent in childrenComponents)
                {
                    yield return childrenComponent;
                }
            }
        }

        public IEnumerable<T> GetComponentsInChildren<T>(bool inherit = false) where T : Component
        {
            return GetComponentsInChildren(typeof(T), inherit) as IEnumerable<T>;
        }

        public Component GetComponentInParent(Type componentType, bool inherit = false)
        {
            var component = GetComponent(componentType, inherit);
            var transform = Transform;
            if (component == null)
            {
                if (transform.Parent != null)
                {
                    return transform.Parent.GameObject.GetComponentInParent(componentType, inherit);
                }
            }
            return component;
        }

        public T GetComponentInParent<T>(bool inherit = false) where T : Component
        {
            return GetComponentInParent(typeof(T), inherit) as T;
        }

        public IEnumerable<Component> GetComponentsInParent(Type componentType, bool inherit = false)
        {
            var currentComponents = GetComponents(componentType, inherit);
            var transform = Transform;
            if (currentComponents != null)
            {
                foreach (var component in currentComponents)
                {
                    yield return component;
                }
            }
            if (transform.Parent != null)
            {
                var parentComponents = transform.Parent.GameObject.GetComponentsInParent(componentType, inherit);
                foreach (var parentComponent in parentComponents)
                {
                    yield return parentComponent;
                }
            }
        }

        public IEnumerable<T> GetComponentsInParent<T>(bool inherit = false) where T : Component
        {
            return GetComponentsInParent(typeof(T), inherit) as List<T>;
        }

        public IEnumerable<Component> GetComponents()
        {
            return Registry.GetComponents(this);
        }
        #endregion Components Controller

        public void Dispose()
        {
            var components = Registry.GetComponents(this);
            foreach (var component in components)
            {
                Registry.Remove(this, component);
            }
            var transform = Transform;
            transform.Parent?.Childrens.Remove(transform);
            foreach (var child in transform.Childrens)
            {
                child.GameObject.Dispose();
            }
        }

        internal void Awake()
        {
            var components = Registry.GetComponents(this);
            foreach (var component in components)
            {
                component.AwakeMethod?.Invoke();
            }
            var transform = Transform;
            foreach (var child in transform.Childrens)
            {
                child.GameObject.Awake();
            }
        }

        internal void OnComponentEnabled(object sender, ComponentEventArgs componentEventArgs)
        {
            ComponentEnabled?.Invoke(this, componentEventArgs);
        }

        internal void OnComponentDisabled(object sender, ComponentEventArgs componentEventArgs)
        {
            ComponentDisabled?.Invoke(this, componentEventArgs);
        }

        internal void Attach(Component component)
        {
            component.GameObject = this;
            component.ComponentEnabled += OnComponentEnabled;
            component.ComponentDisabled += OnComponentDisabled;
            ZIndexChanged += component.OnZIndexChanged;
            ComponentAttached?.Invoke(this, new ComponentEventArgs(component, this));
        }

        internal void Dettach(Component component)
        {
            ComponentDetach?.Invoke(this, new ComponentEventArgs(component, this));
            component.GameObject = null;
            component.ComponentEnabled -= OnComponentEnabled;
            component.ComponentDisabled -= OnComponentDisabled;
        }
    }
}
