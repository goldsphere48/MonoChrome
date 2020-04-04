using MonoChrome.Core.GameObjectSystem.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.GameObjectSystem
{
    public class GameObject : Playable
    {
        private List<Component> _components = new List<Component>();

        private bool _enabled = true;
        public virtual bool Enabled 
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    OnEnable();
                } else
                {
                    OnDisable();
                }
            }
        }

        public GameObject()
        {
            AddComponent<Transform>();
        }

        public T AddComponent<T>() where T : Component
        {
            var component = Activator.CreateInstance(typeof(T)) as Component;
            MethodInfo attachGameObjectMethod = component.GetType().GetMethod("Attach");
            attachGameObjectMethod?.Invoke(component, new object[] { this });
            _components.Add(component);
            component.Awake();
            return component as T;
        }

        public T GetComponent<T>() where T : Component
        {
            return _components.Find(item => item.GetType() == typeof(T)) as T;
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
