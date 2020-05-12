using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
