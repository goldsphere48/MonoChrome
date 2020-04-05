using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Exceptions
{
    class UnfoundRequiredComponentsException : Exception
    {
        public UnfoundRequiredComponentsException(List<Type> requiredComponents) 
            : base($"Can't find required components: {string.Join(",", requiredComponents)}")
        {

        }
    }
}
