using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public abstract class Collider : Component
    {
        public abstract void CheckCollisionWith(Collider collider);
        public abstract bool Contains(Vector2 point);
        internal bool IsMouseOver { get; set; }
        internal bool IsUseRendererBounds { get; set; } = true;
        internal abstract void DrawBounds(SpriteBatch batch);
    }
}