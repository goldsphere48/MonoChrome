using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;

namespace MonoChrome.Core.Components
{
    [RequireComponent(typeof(Transform))]
    class Renderer2D : Renderer
    {
        public Texture2D Texture { get; set; }
        public override Vector2 Size => new Vector2(Texture.Width, Texture.Height);

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, GameObject.Transform.Position, Color);
            }
        }
    }
}
