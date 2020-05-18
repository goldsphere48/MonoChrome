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
            if (_definitions.ContainsKey(definition) == false && definition != null)
            {
                List<Type> componentTypes = new List<Type>();
                componentTypes.AddRange(types);
                if (inheritFromDefinition != null)
                {
                    AddParentTypes(inheritFromDefinition, componentTypes);
                }
                if (componentTypes.Count > 0)
                {
                    _definitions[definition] = componentTypes;
                }
            } else if (definition == null)
            {
                throw new ArgumentNullException();
            } else
            {
                throw new ArgumentException($"Definition {definition} is already exist");
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

        private void AddParentTypes(string parentDefinition, List<Type> componentTypes)
        {
            if (_definitions.ContainsKey(parentDefinition))
            {
                var parentDefinitionComponentTypes = _definitions[parentDefinition];
                foreach (var type in parentDefinitionComponentTypes)
                {
                    if (componentTypes.Contains(type))
                    {
                        componentTypes.Remove(type);
                    }
                }
                componentTypes.AddRange(_definitions[parentDefinition]);
            }
            else
            {
                throw new ArgumentException(
                      $"Can't inherit from definition {parentDefinition}. {parentDefinition} is not define."
                  );
            }
        }
    }
}