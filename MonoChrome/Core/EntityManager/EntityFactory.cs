using MonoChrome.Core.Components;
using System;
using System.Reflection;

namespace MonoChrome.Core.EntityManager
{
    internal class EntityFactory
    {
        public GameObject CreateEmpty()
        {
            return CreateEmpty(GameObject.DefaultName);
        }

        public GameObject CreateEmpty(string name)
        {
            var gameObject = Construct(name);
            gameObject.AddComponent<Transform>();
            return gameObject;
        }

        public GameObject Create(Type[] types)
        {
            return Create(GameObject.DefaultName, types);
        }

        public GameObject Create(string name, Type[] types)
        {
            var gameObject = CreateEmpty(name);
            foreach (var componentType in types)
            {
                gameObject.AddComponent(componentType);
            }
            return gameObject;
        }

        private GameObject Construct(string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var types = new Type[] { typeof(string) };
            var constructor = typeof(GameObject).GetConstructor(flags, null, types, null);
            return constructor?.Invoke(new object[] { name }) as GameObject;
        }
    }
}
