using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public class Collision
    {
        public GameObject GameObject{ get; }

        public Collision(GameObject obj)
        {
            GameObject = obj;
        }
    }
}
