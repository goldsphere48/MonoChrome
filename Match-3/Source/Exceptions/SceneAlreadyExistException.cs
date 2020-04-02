using Match_3.Source.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Exceptions
{
    class SceneAlreadyExistException : Exception
    {
        public SceneAlreadyExistException(SceneId sceneId) : base($"Scene with id: {sceneId.Name} is already exist")
        { 
        
        }
    }
}
