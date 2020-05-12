using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;

namespace MonoChrome.Core.Components
{
    public class SpriteRenderer : Renderer
    {
        public override Vector2 Size => Texture == null ? new Vector2() : new Vector2(Texture.Width, Texture.Height);
        public Texture2D Texture { get; set; }
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
                spriteBatch.Draw(Texture, _transform.Position, null, Color, _transform.Angle, _transform.Origin, Vector2.One, SpriteEffects.None, 1);
            }
        }
        [InsertComponent] private Transform _transform;
    }
}