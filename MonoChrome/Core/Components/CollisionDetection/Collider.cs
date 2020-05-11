using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public abstract class Collider : Component
    {
        public bool DebugDraw { get; set; }
        public abstract void CheckCollisionWith(Collider collider);
        public abstract bool Contains(Vector2 point);
        internal abstract void DrawBounds(SpriteBatch batch);
    }
}
