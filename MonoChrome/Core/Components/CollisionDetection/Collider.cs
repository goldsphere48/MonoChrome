using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public abstract class Collider : Component
    {
        public abstract void CheckCollisionWith(Collider collider);
        public abstract bool Contains(Vector2 point);
    }
}
