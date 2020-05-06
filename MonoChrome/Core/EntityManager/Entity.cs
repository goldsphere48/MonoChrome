using MonoChrome.Core.Helpers;
using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    public static class Entity
    {
        public static EntityStore Registry { private get; set; }

        private static IEntityDefinitionCollection<string> _definitions = new EntityDefinitions();
        private static EntityFactory _entityFactory = new EntityFactory();
        
        public static void Define(string definition, params Type[] componentTypes)
        {
            Define(definition, null, componentTypes);
        }

        public static void Define(string definition, string inheritFromDefinition, params Type[] componentTypes)
        {
            if (definition == null || componentTypes == null)
            {
                throw new ArgumentNullException();
            }
            _definitions.Define(definition, inheritFromDefinition, componentTypes);
        }

        public static GameObject Create(params Component[] components)
        {
            return Create(GameObject.DefaultName, components);
        }

        public static GameObject Create(string name, params Component[] components)
        {
            if (name == null || components == null)
            {
                throw new ArgumentNullException();
            }
            var gameObject = _entityFactory.CreateEmpty(name, Registry);
            AttachComponents(gameObject, components);
            return gameObject;
        }

        public static GameObject CreateFromDefinition(string definition, string name)
        {
            if (definition == null)
            {
                throw new ArgumentNullException();
            }
            var componentTypes = _definitions.Get(definition);
            var gameObject = _entityFactory.Create(name, componentTypes.ToArray(), Registry);
            return gameObject;
        }

        public static void Synchronize()
        {
            Registry.Synchronize();
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
