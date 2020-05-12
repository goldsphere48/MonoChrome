using System;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    internal interface IEntityCollection<TEntity> : IEnumerable<TEntity>
    {
        bool Add(TEntity entity, Component component);
        void Clear();
        bool Contains(TEntity entity);
        T GetComponent<T>(TEntity entity) where T : Component;
        T GetComponent<T>(TEntity entity, bool allowDerivedComponents) where T : Component;
        Component GetComponent(TEntity entity, Type componentType);
        Component GetComponent(TEntity entity, Type componentType, bool allowDerivedComponents);
        IEnumerable<Component> GetComponents(TEntity entity);
        IEnumerable<T> GetComponents<T>() where T : Component;
        IEnumerable<T> GetComponents<T>(bool allowDerivedComponents) where T : Component;
        IEnumerable<Component> GetComponents(Type component);
        IEnumerable<Component> GetComponents(Type component, bool allowDerivedComponents);
        bool Remove(TEntity entity, Component component);
    }
}