using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.Components
{
    public abstract class Renderer : Component
    {
        public Color Color { get; set; } = Color.White;
        public abstract Vector2 Size { get; }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}