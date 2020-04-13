using MonoChrome.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class EntityRegistry
    {
        public EntityStore Store { get; } = new EntityStore();
        public EntityCachedComponents CachedComponents { get; } = new EntityCachedComponents();

        private IDictionary<Predicate<Component>, Action<Component>> _onAddTriggers =
            new Dictionary<Predicate<Component>, Action<Component>>();
        private IDictionary<Predicate<Component>, Action<Component>> _onRemoveTriggers =
            new Dictionary<Predicate<Component>, Action<Component>>();

        public EntityRegistry()
        {
            SetTrigger(
                component => component is Renderer,
                component => CachedComponents.Cache(component),
                component => CachedComponents.Remove(component)
            );
        }

        public void SetTrigger(Predicate<Component> predicate, Action<Component> onAdd, Action<Component> onRemove)
        {
            _onAddTriggers.Add(predicate, onAdd);
            _onRemoveTriggers.Add(predicate, onRemove);
        }
    }
}
