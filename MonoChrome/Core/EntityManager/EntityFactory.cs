using MonoChrome.Core.Components;
using System;

namespace MonoChrome.Core.EntityManager
{
    internal class EntityFactory
    {
        public GameObject CreateEmpty(EntityStore store)
        {
            return CreateEmpty(GameObject.DefaultName, store);
        }
        public GameObject CreateEmpty(string name, EntityStore store)
        {
            var gameObject = new GameObject(name, store);
            gameObject.AddComponent<Transform>();
            gameObject.Transform = gameObject.GetComponent<Transform>();
            return gameObject;
        }
        public GameObject Create(Type[] types, EntityStore store)
        {
            return Create(GameObject.DefaultName, types, store);
        }
        public GameObject Create(string name, Type[] types, EntityStore store)
        {
            var gameObject = CreateEmpty(name, store);
            foreach (var componentType in types)
            {
                gameObject.AddComponent(componentType);
            }
            return gameObject;
        }
    }
}
