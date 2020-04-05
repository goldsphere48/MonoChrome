using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.GameObjectSystem
{
    public class GameObject : Playable
    {
        public Transform Transform { get; }

        private List<Component> _components = new List<Component>();

        public GameObject()
        {
            Transform = AddComponent<Transform>();
        }

        public T AddComponent<T>() where T : Component
        {
            var component = Activator.CreateInstance(typeof(T)) as Component;
            var checkResult = ComponentsAttributeChecker.Verify(component, this);
            if (checkResult)
            {
                component.Attach(this);
                _components.Add(component);
                component.Awake();
                return component as T;
            }
            return null;
        }

        public T GetComponent<T>() where T : Component
        {
            return _components.Find(item => item.GetType() == typeof(T)) as T;
        }

        public Component GetComponent(Type componentType)
        {
            return _components.Find(item => item.GetType() == componentType);
        }

        public void RemoveComponent<T>() where T : Component
        {
            var removableComponent = GetComponent<T>();
            if (removableComponent != null)
            {
                _components.Remove(removableComponent);
            }
        }

        #region IPlayable interface
        public override void Awake()
        {
            
        }

        public override void Start()
        {

        }

        public override void Update()
        {
            for (int i =0; i < _components.Count; ++i)
            {
                _components[i].Update();
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
    }
}
