using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.GameObjectSystem
{
    public sealed class GameObject : Playable
    {
        public Transform Transform { get; }

        private List<Component> _components = new List<Component>();

        public GameObject()
        {
            Transform = AddComponent<Transform>();
        }

        #region Components Controller
        public Component AddComponent(Type componentType)
        {
            var component = Activator.CreateInstance(componentType) as Component;
            var checkResult = ComponentsAttributeChecker.Verify(component, this);
            if (checkResult)
            {
                component.Attach(this);
                _components.Add(component);
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
                _components.Remove(removableComponent);
            }
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public Component GetComponent(Type componentType, bool inherit = false)
        {
            return _components.Find(FindComponentPredicate(componentType, inherit));
        }
        public T GetComponent<T>(bool inherit = false) where T : Component
        {
            return GetComponent(typeof(T), inherit) as T;
        }

        public List<Component> GetComponents(Type componentType, bool inherit = false)
        {
            return _components.FindAll(FindComponentPredicate(componentType, inherit));
        }

        public List<T> GetComponents<T>() where T : Component
        {
            return GetComponents(typeof(T)) as List<T>;
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

        public List<Component> GetComponentsInChildren(Type componentType, bool inherit = false)
        {
            var components = GetComponents(componentType, inherit);
            if (components.Count == 0)
            {
                for (int i = 0; i < Transform.Childrens.Count; ++i)
                {
                    components.AddRange(Transform.Childrens[i].GameObject.GetComponentsInChildren(componentType));
                }
            }
            return components;
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

        public List<Component> GetComponentsInParent(Type componentType, bool inherit = false)
        {
            var components = GetComponents(componentType, inherit);
            if (components.Count == 0)
            {
                if (Transform.Parent != null)
                {
                    var parentComponents = Transform.Parent.GameObject.GetComponentsInParent(componentType, inherit);
                    components.AddRange(parentComponents);
                }
            }
            return components;
        }

        public List<T> GetComponentsInParent<T>(bool inherit = false) where T : Component
        {
            return GetComponentsInParent(typeof(T), inherit) as List<T>;
        }
        #endregion Components Controller

        #region IPlayable interface
        public override void Awake()
        {

        }

        public override void Start()
        {

        }

        public override void Update()
        {
            for (int i = 0; i < _components.Count; ++i)
            {
                _components[i].Update();
            }
            for (int i = 0; i < Transform.Childrens.Count; ++i)
            {
                Transform.Childrens[i].Update();
            }

        }

        public override void OnEnable()
        {

        }

        public override void OnDisable()
        {

        }

        public override void OnDestroy()
        {

        }

        public override void OnFinalize()
        {

        }
        #endregion

        private Predicate<Component> FindComponentPredicate(Type componentType, bool inherit)
        {
            return item => item.GetType() == componentType ||
               (inherit && item.GetType().IsAssignableFrom(componentType));
        }
    }
}
