using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    interface IEntityCollection<TEntity> : IEnumerable<TEntity>
    {
        event EventHandler ComponentAdded;
        event EventHandler ComponentRemoved;
        event EventHandler ComponentEnabled;
        event EventHandler ComponentDisabled;
        void Add(TEntity entity, Component component);
        bool Remove(TEntity entity, Component component);
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
    }
}
