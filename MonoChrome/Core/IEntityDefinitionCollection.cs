using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    interface IEntityDefinitionCollection<TEntityDefinition> : IEnumerable<TEntityDefinition>
    {
        void Define(TEntityDefinition definition, params Type[] types);
        void Define(string definition, string inheritFromDefinition, params Type[] types);
        bool Undefine(TEntityDefinition definition);
        void Clear();
        GameObject Make(TEntityDefinition definition);
    }
}
