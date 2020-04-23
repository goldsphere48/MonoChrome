using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    static class ComponentExtensions
    {
        public static void AddComponent(this Component component, Type componentType)
        {
            component.GameObject.AddComponent(componentType);
        }
        public static void AddComponent(this Component component, Component newComponent)
        {
            component.GameObject.AddComponent(newComponent);
        }
        public static void AddComponent<T>(this Component component) where T : Component
        {
            component.GameObject.AddComponent<T>();
        }
        public static void RemoveComponent(this Component component, Type type)
        {
            component.GameObject.RemoveComponent(type);
        }
        public static void RemoveComponent<T>(this Component component) where T : Component
        {
            component.GameObject.RemoveComponent<T>();
        }
        public static Component GetComponent(this Component component, Type componentType, bool inherit = false)
        {
            return component.GameObject.GetComponent(componentType, inherit);
        }
        public static T GetComponent<T>(this Component component, bool inherit = false) where T : Component
        {
            return component.GameObject.GetComponent(typeof(T), inherit) as T;
        }
        public static Component GetComponentInChildren(this Component component, Type componentType, bool inherit = false)
        {
            return component.GameObject.GetComponentInChildren(componentType, inherit);
        }
        public static T GetComponentInChildren<T>(this Component component, bool inherit = false) where T : Component
        {
            return component.GameObject.GetComponentInChildren<T>(inherit);
        }
        public static IEnumerable<Component> GetComponentsInChildren(this Component component, Type componentType, bool inherit = false)
        {
            return component.GameObject.GetComponentsInChildren(componentType, inherit);
        }
        public static IEnumerable<T> GetComponentsInChildren<T>(this Component component, bool inherit = false) where T : Component
        {
            return component.GameObject.GetComponentsInChildren(typeof(T), inherit) as List<T>;
        }
        public static Component GetComponentInParent(this Component component, Type componentType, bool inherit = false)
        {
            return component.GameObject.GetComponentInParent(componentType, inherit);
        }
        public static T GetComponentInParent<T>(this Component component, bool inherit = false) where T : Component
        {
            return component.GameObject.GetComponentInParent<T>(inherit);
        }
        public static IEnumerable<Component> GetComponentsInParent(this Component component, Type componentType, bool inherit = false)
        {
            return component.GameObject.GetComponentsInParent(componentType, inherit);
        }
        public static IEnumerable<T> GetComponentsInParent<T>(this Component component, bool inherit = false) where T : Component
        {
            return component.GameObject.GetComponentsInParent<T>(inherit);
        }
        public static IEnumerable<Component> GetComponents(this Component component)
        {
            return component.GameObject.GetComponents();
        }
    }
}