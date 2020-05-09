using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    public class EntityStore : IEntityCollection<GameObject>
    {
        private IDictionary<GameObject, IDictionary<Type, Component>> _gameObjects =
            new Dictionary<GameObject, IDictionary<Type, Component>>();
        private ICollection<Component> _unsynchronizedComponents =
            new HashSet<Component>();
        private IDictionary<Predicate<Component>, Action<Component>> _triggers =
            new Dictionary<Predicate<Component>, Action<Component>>();

        public EntityStore()
        {
            SetTrigger(
                component => true,
                component => {
                    var issues = ComponentAttributeAplicator.Apply(component);
                    foreach (var issue in issues)
                    {
                        throw new Exception(issue.Message);
                    }
                }
            );
        }

        public bool Add(GameObject entity, Component component)
        {
            if (entity == null || component == null)
            {
                throw new ArgumentNullException();
            }
            bool componentSuccessfullyAttached = false;
            var components = GetComponentsForEntity(entity);
            if (components == null)
            {
                components = new Dictionary<Type, Component>();
                _gameObjects[entity] = components;
            }
            if (!components.ContainsKey(component.GetType()))
            {
                components.Add(component.GetType(), component);
                entity.Attach(component);
                componentSuccessfullyAttached = true;
                _unsynchronizedComponents.Add(component);
            }
            return componentSuccessfullyAttached;
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
                if (components.Count == 0)
                {
                    _gameObjects.Remove(entity);
                }
                return false;
            }
            entity.Dettach(component);
            component.Dispose();
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

        public bool HasComponent<T>(GameObject gameObject, bool inherit = false) where T : Component
        {
            if (gameObject != null && Contains(gameObject))
            {
                if (inherit == false)
                {
                    return _gameObjects[gameObject].ContainsKey(typeof(T));
                } else
                {
                    return GetComponent<T>(gameObject, inherit) != null;
                }
            }
            return false;
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

        public void Clear()
        {
            _gameObjects.Clear();
        }

        public void SetTrigger(Predicate<Component> predicate, Action<Component> action)
        {
            _triggers.Add(predicate, action);
        }

        public void Synchronize()
        {
            foreach (var trigger in _triggers)
            {
                foreach (var component in _unsynchronizedComponents)
                {
                    if (trigger.Key(component))
                    {
                        trigger.Value(component);
                    }
                }
            }
            _unsynchronizedComponents.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
