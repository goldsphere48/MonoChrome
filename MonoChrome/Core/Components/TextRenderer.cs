using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;

namespace MonoChrome.Core.Components
{
    public class TextRenderer : GameObjectRenderer
    {
        public override Vector2 Size => _size;
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
                    _transform.Origin = Center;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SpriteFont != null)
            {
                spriteBatch.DrawString(SpriteFont, Text, _transform.Position, Color, _transform.Angle, _transform.Origin, Vector2.One, SpriteEffects.None, 1);
            }
        }
        private Vector2 _size;
        private string _text = "";
        [InsertComponent] private Transform _transform;
    }
}