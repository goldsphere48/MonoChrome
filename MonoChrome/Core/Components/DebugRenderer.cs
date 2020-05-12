using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using MonoChrome.Core.Components.CollisionDetection;

namespace MonoChrome.Core.Components
{
    public class DebugRenderer : Renderer
    {
        public override Vector2 Size => new Vector2();
        [InsertComponent(Inherit = true, Required = false)]
        private Collider _collider;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_collider != null)
            {
                _collider.DrawBounds(spriteBatch);
            }
        }
    }
}
