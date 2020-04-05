using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Exceptions
{
    public class UnfoundRequiredComponentsException : Exception
    {
        public UnfoundRequiredComponentsException(Type requiredComponent) 
            : base($"Can't find required components: {requiredComponent}")
        {

        }
    }
}
