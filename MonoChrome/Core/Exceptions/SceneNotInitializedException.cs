using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Exceptions
{
    class SceneNotInitializedException : Exception
    {
        public SceneNotInitializedException(Type sceneType) : base($"Scene with id: {sceneType.Name} not initialized")
        { 
        
        }
    }
}
