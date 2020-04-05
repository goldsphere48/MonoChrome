using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Exceptions
{
    public class InvalidComponentDuplicateException : Exception
    {
        public InvalidComponentDuplicateException(Type componentType) 
            : base($"Try to add duplicate of {componentType.Name}. This component can only be one.")
        {

        }
    }
}
