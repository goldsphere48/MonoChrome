using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);
    public class ComponentEventArgs : EventArgs
    {
        public GameObject GameObject { get; private set; }
        public Component Component { get; private set; }
        public ComponentEventArgs(Component component, GameObject gameObject)
        {
            GameObject = gameObject;
            Component = component;
        }
    }
}
