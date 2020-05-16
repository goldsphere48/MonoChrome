using MonoChrome.Core.Helpers.FieldInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    internal class EntityStore : IEntityCollection<GameObject>
    {
        private FieldInjector _injector = new FieldInjector();

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
                _injector.OnObjectEntered(entity);
            }
            if (!components.ContainsKey(component.GetType()))
            {
                components.Add(component.GetType(), component);
                entity.Attach(component);
                _injector.OnComponentAdded(component);
                componentSuccessfullyAttached = true;
            }
            return componentSuccessfullyAttached;
        }

        internal IEnumerable<AttributeError> GetIssues(Component component)
        {
            return _injector.GetIssues(component);
        }

        public void Clear()
        {
            foreach (var gameObject in _gameObjects.Keys)
            {
                EraseGameObject(gameObject);
            }
            _gameObjects.Clear();
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
            return GetComponentsForEntity(entity)?.Values;
        }
        public IEnumerable<Component> GetComponents(GameObject entity, Type type, bool inherit)
        {
            if (!inherit)
            {
                yield return GetComponent(entity, type);
                yield break;
            }
            var componentsTypes = GetComponentsForEntity(entity)?.Keys;
            foreach (var componentType in componentsTypes)
            {
                if (type.IsAssignableFrom(componentType))
                {
                    yield return _gameObjects[entity][componentType];
                }
            }
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
        public bool HasComponent<T>(GameObject gameObject, bool inherit = false) where T : Component
        {
            if (gameObject != null && Contains(gameObject))
            {
                if (inherit == false)
                {
                    return _gameObjects[gameObject].ContainsKey(typeof(T));
                }
                else
                {
                    return GetComponent<T>(gameObject, inherit) != null;
                }
            }
            return false;
        }

        private void EraseComponent(GameObject entity, Component component)
        {
            _injector.OnComponentRemove(component);
            entity.Dettach(component);
            component.Dispose();
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
                    _injector.OnObjectDrop(entity);
                    _gameObjects.Remove(entity);
                }
                return false;
            }
            EraseComponent(entity, component);
            return components.Remove(component.GetType());
        }
        public bool Remove(GameObject entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            EraseGameObject(entity);
            return _gameObjects.Remove(entity);
        }
        internal IDictionary<Type, Component> GetComponentsForEntity(GameObject gameObject)
        {
            IDictionary<Type, Component> components = null;
            if (_gameObjects.ContainsKey(gameObject))
            {
                components = _gameObjects[gameObject];
            }
            return components;
        }
        private IDictionary<GameObject, IDictionary<Type, Component>> _gameObjects = new Dictionary<GameObject, IDictionary<Type, Component>>();

        private void EraseGameObject(GameObject entity)
        {
            var components = GetComponentsForEntity(entity);
            foreach (var component in components.Values)
            {
                EraseComponent(entity, component);
            }
            components.Clear();
            _injector.OnObjectDrop(entity);
            _gameObjects.Remove(entity);
        }
    }
}