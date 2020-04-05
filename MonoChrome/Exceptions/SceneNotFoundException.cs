using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Exceptions
{
    public class SceneNotFoundException : Exception
    {
        public SceneNotFoundException(Type sceneType) : base($"Scene with id: {sceneType.Name} not founded")
        { 
        
        }
    }
}
