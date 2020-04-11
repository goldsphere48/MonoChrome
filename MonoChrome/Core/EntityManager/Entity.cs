using MonoChrome.Core.Helpers;
using MonoChrome.Core.Helpers.ComponentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    static class Entity
    {
        private static IEntityDefinitionCollection<string> _definitions = new EntityDefinitions();
        private static EntityFactory _entityFactory = new EntityFactory();

        public static void Define(string definition, params Type[] componentTypes)
        {
            Define(definition, null, componentTypes);
        }

        public static void Define(string definition, string inheritFromDefinition, params Type[] componentTypes)
        {
            if (definition == null || componentTypes == null || inheritFromDefinition == null)
            {
                throw new ArgumentNullException();
            }
            if (ComponentValidator.Valid(componentTypes))
            {
                _definitions.Define(definition, inheritFromDefinition, componentTypes);
            }
        }

        public static GameObject Create(string name, params Type[] componentTypes)
        {
            if (name == null || componentTypes == null)
            {
                throw new ArgumentNullException();
            }
            if (ComponentValidator.Valid(componentTypes))
            {
                return _entityFactory.Create(name, componentTypes);
            }
            return null;
        }

        public static GameObject CreateFromDefinition(string definition, string name)
        {
            if (definition == null)
            {
                throw new ArgumentNullException();
            }
            var componentTypes = _definitions.Get(definition);
            return _entityFactory.Create(name, componentTypes.ToArray());
        }
    }
}
