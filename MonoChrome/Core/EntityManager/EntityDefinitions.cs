using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    internal class EntityDefinitions : IEntityDefinitionCollection<string>
    {
        private IDictionary<string, IList<Type>> _definitions = new Dictionary<string, IList<Type>>();

        public void Clear()
        {
            _definitions.Clear();
        }

        public void Define(string definition, string inheritFromDefinition, params Type[] types)
        {
            if (_definitions.ContainsKey(definition))
            {
                throw new ArgumentException($"Definition {definition} is already exist");
            }
            List<Type> componentTypes = new List<Type>();
            componentTypes.AddRange(types);
            if (inheritFromDefinition != null)
            {
                if (!_definitions.ContainsKey(inheritFromDefinition))
                {
                    throw new ArgumentException(
                        $"Can't inherit from definition {inheritFromDefinition}. {inheritFromDefinition} is not define."
                    );
                }
                else
                {
                    var parentDefinitionComponentTypes = _definitions[inheritFromDefinition];
                    foreach (var type in parentDefinitionComponentTypes)
                    {
                        if (componentTypes.Contains(type))
                        {
                            componentTypes.Remove(type);
                        }
                    }
                    componentTypes.AddRange(_definitions[inheritFromDefinition]);
                }
            }
            if (componentTypes.Count > 0)
            {
                _definitions[definition] = componentTypes;
            }
        }

        public IList<Type> Get(string definition)
        {
            return _definitions[definition];
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _definitions.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Undefine(string definition)
        {
            return _definitions.Remove(definition);
        }
    }
}