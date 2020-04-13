using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    static class EntityRegistry
    {
        public static EntityStore Global = Create();
        public static EntityStore Current = Global;

        private static IDictionary<object, IEntityCollection<GameObject>> _stores = 
            new Dictionary<object, IEntityCollection<GameObject>>();

        public static void SetContext(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }
            _stores.TryGetValue(context, out IEntityCollection<GameObject> store);
            if (store == null)
            {
                store = Create();
                _stores.Add(context, store);
            }
            Current = store as EntityStore;
        }

        public static EntityStore Create()
        {
            return new EntityStore();
        }
    }
}
