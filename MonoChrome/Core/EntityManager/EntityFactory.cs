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
            var gameObject = new GameObject(name);
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
    }
}
