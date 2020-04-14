using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.Core.EntityManager
{
    class EntityStore : IEntityCollection<GameObject>
    {
        private IDictionary<GameObject, IDictionary<Type, Component>> _gameObjects =
            new Dictionary<GameObject, IDictionary<Type, Component>>();

        public event EventHandler ComponentAdded;
        public event EventHandler ComponentRemoved;

        public void Add(GameObject entity, Component component)
        {
            if (entity == null || component == null)
            {
                throw new ArgumentNullException();
            }
            var components = GetComponentsForEntity(entity);
            if (components == null)
            {
                components = new Dictionary<Type, Component>();
                _gameObjects[entity] = components;
            }
            components.Add(component.GetType(), component);
            component.Attach(entity);
            OnAdd(component, entity);
        }

        public bool Remove(GameObject entity, Component component)
        {
            if (entity == null || component == null)
            {
                throw new ArgumentNullException();
            }
            var components = GetComponentsForEntity(entity);
            if (components == null || components.Count == 0)
            {
                return false;
            }
            component.Dettach();
            //TODO Component dispose
            return components.Remove(component.GetType());
        }

        IDictionary<Type, Component> GetComponentsForEntity(GameObject gameObject)
        {
            IDictionary<Type, Component> components = null;
            if (_gameObjects.ContainsKey(gameObject))
            {
                components = _gameObjects[gameObject];
            }
            return components;
        }

        public bool Contains(GameObject entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            return _gameObjects.ContainsKey(entity);
        }

        public T GetComponent<T>(GameObject entity) where T : Component
        {
            return GetComponent(entity, typeof(T), false) as T;
        }

        public T GetComponent<T>(GameObject entity, bool allowDerivedComponents) where T : Component
        {
            return GetComponent(entity, typeof(T), allowDerivedComponents) as T;
        }

        public Component GetComponent(GameObject entity, Type componentType)
        {
            return GetComponent(entity, componentType, false);
        }

        public Component GetComponent(GameObject entity, Type componentType, bool allowDerivedComponents)
        {
            if (entity == null || componentType == null)
            {
                throw new ArgumentNullException();
            }

            IDictionary<Type, Component> components = GetComponentsForEntity(entity);

            Component result = null;

            if (components != null && components.Count > 0)
            {
                foreach (Component otherComponent in components.Values)
                {
                    if ((allowDerivedComponents && otherComponent.GetType().IsSubclassOf(componentType))
                        || otherComponent.GetType() == componentType)
                    {
                        result = otherComponent;

                        break;
                    }
                }
            }

            return result;
        }

        public IEnumerable<Component> GetComponents(GameObject entity)
        {
            return GetComponentsForEntity(entity).Values;
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return GetComponents(typeof(T), false) as IEnumerable<T>;
        }

        public IEnumerable<T> GetComponents<T>(bool allowDerivedComponents) where T : Component
        {
            return GetComponents(typeof(T), allowDerivedComponents) as IEnumerable<T>;
        }

        public IEnumerable<Component> GetComponents(Type component)
        {
            return GetComponents(component, false);
        }

        public IEnumerable<Component> GetComponents(Type component, bool allowDerivedComponents)
        {
            foreach (var go in _gameObjects.Keys)
            {
                yield return GetComponent(go, component, allowDerivedComponents);
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnAdd(Component component, GameObject gameObject)
        {
            var args = new ComponentEventArgs(component, gameObject);
            ComponentAdded?.Invoke(this, args);
        }

        private void OnRemove(Component component, GameObject gameObject)
        {
            var args = new ComponentEventArgs(component, gameObject);
            ComponentRemoved?.Invoke(this, args);
        }
    }
}
