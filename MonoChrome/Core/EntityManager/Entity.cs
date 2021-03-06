﻿using MonoChrome.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.Core.EntityManager
{
    public static class Entity
    {
        internal static EntityStore Registry { get; set; }
        private static IEntityDefinitionCollection<string> _definitions = new EntityDefinitions();
        private static EntityFactory _entityFactory = new EntityFactory();

        public static GameObject Compose(GameObject parent, params GameObject[] childrens)
        {
            foreach (var child in childrens)
            {
                child.Transform.Parent = parent.Transform;
            }
            return parent;
        }

        public static GameObject Compose(string name, params GameObject[] childrens)
        {
            var parent = _entityFactory.CreateEmpty(name, Registry);
            return Compose(parent, childrens);
        }

        public static GameObject ComposeNew(params GameObject[] childrens)
        {
            return Compose(GameObject.DefaultName, childrens);
        }

        public static GameObject ComposeNew(string name, params GameObject[] childrens)
        {
            return Compose(name, childrens);
        }

        public static GameObject Create(params Component[] components)
        {
            return Create(GameObject.DefaultName, components);
        }

        public static GameObject Create(string name, params Component[] components)
        {
            if (name != null && components != null)
            {
                var gameObject = _entityFactory.CreateEmpty(name, Registry);
                AttachComponents(gameObject, components);
                return gameObject;
            } else
            {
                throw new ArgumentNullException();
            }
        }

        public static GameObject CreateFromDefinition(string definition, string name)
        {
            if (definition != null)
            {
                var componentTypes = _definitions.Get(definition);
                var gameObject = _entityFactory.Create(name, componentTypes.ToArray(), Registry);
                return gameObject;
            } else
            {
                throw new ArgumentNullException();
            }

        }

        public static IEnumerable<GameObject> Decompose(GameObject gameObject)
        {
            var childrens = new List<Transform>(gameObject.Transform.Childrens);
            foreach (var child in childrens)
            {
                child.Parent = null;
                Decompose(child.GameObject);
                yield return child.GameObject;
            }
        }

        public static void Define(string definition, params Type[] componentTypes)
        {
            Define(definition, null, componentTypes);
        }

        public static void Define(string definition, string inheritFromDefinition, params Type[] componentTypes)
        {
            if (definition != null && componentTypes != null)
            {
                _definitions.Define(definition, inheritFromDefinition, componentTypes);
            } else
            {
                throw new ArgumentNullException();
            }
        }

        public static GameObject Find(string name)
        {
            foreach (var gameObject in Registry)
            {
                if (gameObject.Name == name)
                {
                    return gameObject;
                }
            }
            return null;
        }

        public static IEnumerable<GameObject> FindAll(string name)
        {
            var result = new List<GameObject>();
            foreach (var gameObject in Registry)
            {
                if (gameObject.Name == name)
                {
                    result.Add(gameObject);
                }
            }
            return result;
        }

        private static void AttachComponents(GameObject gameObject, IEnumerable<Component> components)
        {
            foreach (var component in components)
            {
                Registry.Add(gameObject, component);
            }
        }
    }
}