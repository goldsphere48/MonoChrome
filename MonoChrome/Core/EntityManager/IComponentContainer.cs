using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    interface IComponentContainer
    {
        void AddComponent(Type componentType);
        void AddComponent<T>() where T : Component;
        bool RemoveComponent(Type componentType);
        bool RemoveComponent<T>() where T : Component;
        Component GetComponent(Type componentType, bool inherit = false);
        T GetComponent<T>(bool inherit = false) where T : Component;
        Component GetComponentInChildren(Type componentType, bool inherit = false);
        T GetComponentInChildren<T>(bool inherit = false) where T : Component;
        Component GetComponentInParent(Type componentType, bool inherit = false);
        T GetComponentInParent<T>(bool inherit = false) where T : Component;
        IEnumerable<T> GetComponents<T>(bool inherit = false) where T : Component;
        IEnumerable<Component> GetComponents(Type componentType, bool inherit = false);
        IEnumerable<Component> GetComponentsInChildren(Type componentType, bool inherit = false);
        IEnumerable<T> GetComponentsInChildren<T>(bool inherit = false) where T : Component;
        IEnumerable<Component> GetComponentsInParent(Type componentType, bool inherit = false);
        IEnumerable<T> GetComponentsInParent<T>(bool inherit = false) where T : Component;
    }
}
