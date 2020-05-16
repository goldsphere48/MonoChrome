using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public abstract class Collider : Component
    {
        public bool CheckCollision { get; set; } = false;
        internal bool IsMouseOver { get; set; }
        internal bool IsUseRendererBounds { get; set; } = true;

        public abstract void CheckCollisionWith(Collider collider);

        public abstract bool Contains(Vector2 point);

        internal abstract void DrawBounds(SpriteBatch batch);
    }
}