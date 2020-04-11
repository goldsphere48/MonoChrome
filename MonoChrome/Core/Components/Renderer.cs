using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components
{
    abstract class Renderer : Component
    {
        public Color Color { get; set; } = Color.White;
        public abstract Vector2 Size { get; }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
