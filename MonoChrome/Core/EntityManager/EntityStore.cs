using MonoChrome.Core.Helpers.FieldInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    internal class EntityStore : IEntityCollection<GameObject>
    {
        private IDictionary<GameObject, IDictionary<Type, Component>> _gameObjects = new Dictionary<GameObject, IDictionary<Type, Component>>();
        private FieldInjector _injector = new FieldInjector();

        public bool Add(GameObject entity, Component component)
        {
            if (entity != null && component != null)
            {
                bool componentSuccessfullyAttached = false;
                var components = GetComponentsForEntity(entity);
                if (components == null)
                {
                    components = new Dictionary<Type, Component>();
                    _gameObjects[entity] = components;
                    _injector.OnObjectEntered(entity);
                }
                if (components.ContainsKey(component.GetType()) == false)
                {
                    components.Add(component.GetType(), component);
                    entity.Attach(component);
                    _injector.OnComponentAdded(component);
                    componentSuccessfullyAttached = true;
                }
                return componentSuccessfullyAttached;
            } else
            {
                throw new ArgumentNullException();
            }
            
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
            if (entity != null)
            {
                return _gameObjects.ContainsKey(entity);
            } else
            {
                throw new ArgumentNullException();
            }
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
            if (entity != null && componentType != null)
            {
                IDictionary<Type, Component> components = GetComponentsForEntity(entity);
                Component result = null;
                if (components != null && components.Count > 0)
                {
                    foreach (Component otherComponent in components.Values)
                    {
                        if (IsMatch(allowDerivedComponents, componentType, otherComponent))
                        {
                            result = otherComponent;
                            break;
                        }
                    }
                }
                return result;
            } else
            {
                throw new ArgumentNullException();
            }
        }

        public IEnumerable<Component> GetComponents(GameObject entity)
        {
            return GetComponentsForEntity(entity)?.Values;
        }

        public IEnumerable<Component> GetComponents(GameObject entity, Type type, bool inherit)
        {
            if (inherit == false)
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

        public bool Remove(GameObject entity, Component component)
        {
            if (entity != null && component != null)
            {
                var components = GetComponentsForEntity(entity);
                if (components != null)
                {
                    if (components.Count == 0)
                    {
                        _injector.OnObjectDrop(entity);
                        _gameObjects.Remove(entity);
                    }
                    else
                    {
                        EraseComponent(entity, component);
                        return components.Remove(component.GetType());
                    }
                }
                return false;
            } else
            {
                throw new ArgumentNullException();
            }
        }

        public bool Remove(GameObject entity)
        {
            if (entity != null)
            {
                EraseGameObject(entity);
                return _gameObjects.Remove(entity);
            } else
            {
                throw new ArgumentNullException();
            }
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

        internal IEnumerable<AttributeError> GetIssues(Component component)
        {
            return _injector.GetIssues(component);
        }

        private void EraseComponent(GameObject entity, Component component)
        {
            _injector.OnComponentRemove(component);
            entity.Dettach(component);
            component.Dispose();
        }

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

        private bool IsMatch(bool allowDerivedComponents, Type targetType, Component matchingComponent)
        {
            return (allowDerivedComponents && matchingComponent.GetType().IsSubclassOf(targetType))
                            || matchingComponent.GetType() == targetType;
        }
    }
}