using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Exceptions
{
    public class InvalidComponentTargetException : Exception
    {
        public InvalidComponentTargetException(Type componentType, Type gameObjectType)
            : base($"Component {componentType.Name} target must be {gameObjectType.Name}")
        {

        }
    }
}
