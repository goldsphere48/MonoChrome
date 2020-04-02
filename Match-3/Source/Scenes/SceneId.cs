using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Scenes
{
    class SceneId
    {
        public readonly string Name;

        public SceneId(string sceneName)
        {
            Name = sceneName;
        }

        public override bool Equals(object obj)
        {
            if (obj is SceneId)
            {
                return (obj as SceneId).Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
