using System;
using System.Collections.Generic;

namespace MonoChrome.Core.EntityManager
{
    internal interface IEntityDefinitionCollection<TEntityDefinition> : IEnumerable<TEntityDefinition>
    {
        void Clear();
        void Define(TEntityDefinition definition, TEntityDefinition inheritFromDefinition, params Type[] types);
        IList<Type> Get(TEntityDefinition definition);
        bool Undefine(TEntityDefinition definition);
    }
}