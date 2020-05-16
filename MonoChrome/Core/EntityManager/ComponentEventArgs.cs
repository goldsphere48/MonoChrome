using System;

namespace MonoChrome.Core.EntityManager
{
    public class ComponentEventArgs : EventArgs
    {
        public Component Component { get; private set; }
        public GameObject GameObject { get; private set; }

        public ComponentEventArgs(Component component, GameObject gameObject)
        {
            GameObject = gameObject;
            Component = component;
        }
    }

    public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);
}