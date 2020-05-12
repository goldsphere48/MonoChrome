using System;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    interface IEntityDefinitionCollection<TEntityDefinition> : IEnumerable<TEntityDefinition>
    {
        void Define(TEntityDefinition definition, TEntityDefinition inheritFromDefinition, params Type[] types);
        bool Undefine(TEntityDefinition definition);
        IList<Type> Get(TEntityDefinition definition);
        void Clear();
    }
}
