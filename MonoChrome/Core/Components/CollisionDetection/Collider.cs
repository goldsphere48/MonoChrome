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
        internal bool IsMouseOver { get; set; }
        public abstract void CheckCollisionWith(Collider collider);
        public abstract bool Contains(Vector2 point);
        internal abstract void DrawBounds(SpriteBatch batch);
        internal bool IsUseRendererBounds { get; set; } = true;
    }
}
