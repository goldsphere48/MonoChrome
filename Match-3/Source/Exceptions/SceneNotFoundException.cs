using Match_3.Source.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Exceptions
{
    class SceneNotFoundException : Exception
    {
        public SceneNotFoundException(SceneId sceneId) : base($"Scene with id: {sceneId.Name} not founded")
        { 
        
        }
    }
}
