using MonoChrome.Core.Components;
using MonoChrome.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MonoChrome.Core.Helpers.ComponentValidation;
using MonoChrome.Core.EntityManager;

namespace MonoChrome.Core
{
    public sealed class GameObject
    {
        public const string DefaultName = "GameObject";
        public Transform Transform { get; }
        public string Name { get; }

        internal EntityRegistry Registry { get; set; }

        private GameObject(string name)
        {
            Name = name;
        }

        #region Components Controller
        public Component AddComponent(Type componentType)
        {
            var component = Component.Create(componentType);
            var components = Registry.Store.GetComponents(this).ToList();
            if (ComponentValidator.Valid(component, components))
            {
                Registry.Store.Add(this, component);
                component.Awake();
                return component;
            }
            return null;
        }

        public T AddComponent<T>() where T : Component
        {
            return AddComponent(typeof(T)) as T;
        }

        public void RemoveComponent(Type type)
        {
            var removableComponent = GetComponent(type);
            if (removableComponent != null)
            {
                EntityRegistry.Current.Remove(this, removableComponent);
            }
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public Component GetComponent(Type componentType, bool inherit = false)
        {
            return EntityRegistry.Current.GetComponent(this, componentType, inherit);
        }
        public T GetComponent<T>(bool inherit = false) where T : Component
        {
            return GetComponent(typeof(T), inherit) as T;
        }

        public Component GetComponentInChildren(Type componentType, bool inherit = false)
        {
            var component = GetComponent(componentType, inherit);
            if (component == null)
            {
                for (int i = 0; i < Transform.Childrens.Count; ++i)
                {
                    component = Transform.Childrens[i].GameObject.GetComponentInChildren(componentType);
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
            if (currentComponent != null)
            {
                yield return currentComponent;
            }
            foreach (var child in Transform.Childrens)
            {
                var childrenComponents = child.GameObject.GetComponentsInChildren(componentType, inherit);
                foreach (var childrenComponent in childrenComponents)
                {
                    yield return childrenComponent;
                }
            }
        }

        public List<T> GetComponentsInChildren<T>(bool inherit = false) where T : Component
        {
            return GetComponentsInChildren(typeof(T), inherit) as List<T>;
        }

        public Component GetComponentInParent(Type componentType, bool inherit = false)
        {
            var component = GetComponent(componentType, inherit);
            if (component == null)
            {
                if (Transform.Parent != null)
                {
                    return Transform.Parent.GameObject.GetComponentInParent(componentType, inherit);
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
            if (currentComponent != null)
            {
                yield return currentComponent;
            }
            if (Transform.Parent != null)
            {
                var parentComponents = Transform.Parent.GameObject.GetComponentsInParent(componentType, inherit);
                foreach (var parentComponent in parentComponents)
                {
                    yield return parentComponent;
                }
            }
        }

        public List<T> GetComponentsInParent<T>(bool inherit = false) where T : Component
        {
            return GetComponentsInParent(typeof(T), inherit) as List<T>;
        }
        #endregion Components Controller
    }
}
