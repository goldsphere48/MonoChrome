using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using MonoChrome.Core.Components.CollisionDetection;

namespace MonoChrome.Core.Components
{
    public class SpriteRenderer : Renderer
    {
        public Texture2D Texture { get; set; }
        public override Vector2 Size => new Vector2(Texture.Width, Texture.Height);

        [InsertComponent(Inherit = true)]
        private Collider Collider;

        public SpriteRenderer()
        {

        }

        public SpriteRenderer(Texture2D texture)
        {
            Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, GameObject.Transform.Position, Color);
            }
            if (Collider.DebugDraw)
            {
                Collider.DrawBounds(spriteBatch);
            }
        }
    }
}
