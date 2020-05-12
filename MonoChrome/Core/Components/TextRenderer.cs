using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;

namespace MonoChrome.Core.Components
{
    public class TextRenderer : Renderer
    {
        public SpriteFont SpriteFont { get; set; }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (SpriteFont != null && _text != null)
                {
                    _size = SpriteFont.MeasureString(_text);
                }
            }
        }
        public override Vector2 Size => _size;
        [InsertComponent]
        private Transform _transform;
        private string _text = "";
        private Vector2 _size;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SpriteFont != null)
            {
                spriteBatch.DrawString(SpriteFont, Text, _transform.Position, Color);
            }
        }
    }
}
