using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    static class Entity
    {
        public static IEntityDefinitionCollection<string> _definitions = new EntityDefinitions();
        public static void Define(string name, params Type[] componentTypes)
        {
            _definitions.Define(name, componentTypes);
        }

        public static void Define(string name, string inheritFromDefinition, params Type[] componentTypes)
        {
            _definitions.Define(name, inheritFromDefinition, componentTypes);
        }

        public static GameObject Create(string name, params Type[] componentTypes)
        {
            var gameObject = new GameObject(name);
            foreach (var componentType in componentTypes)
            {
                gameObject.AddComponent(componentType);
            }
            return gameObject;
        }

        public static GameObject CreateFromDefinition(string name)
        {
            return _definitions.Make(name);
        }
    }
}
