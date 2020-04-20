using MonoChrome.Core.Components;
using MonoChrome.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using MonoChrome.Core.EntityManager;

namespace MonoChrome.Core
{
    public sealed class GameObject
    {
        public const string DefaultName = "GameObject";
        public Transform Transform { get => GetComponent<Transform>(); }
        public string Name { get; }

        internal EntityStore Registry { get; set; }

        internal GameObject(string name)
        {
            Name = name;
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

        public Component GetComponent(Type componentType, bool inherit = false)
        {
            return Registry.GetComponent(this, componentType, inherit);
        }
        public T GetComponent<T>(bool inherit = false) where T : Component
        {
            return GetComponent(typeof(T), inherit) as T;
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
            var currentComponent = GetComponent(componentType, inherit);
            var transform = Transform;
            if (currentComponent != null)
            {
                yield return currentComponent;
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
            return GetComponentsInChildren(typeof(T), inherit) as List<T>;
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
            var currentComponent = GetComponent(componentType, inherit);
            var transform = Transform;
            if (currentComponent != null)
            {
                yield return currentComponent;
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
    }
}
