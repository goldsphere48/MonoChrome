using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using System;

namespace MonoChrome.Core.Components
{
    public class SpriteRenderer : GameObjectRenderer
    {
        public override Vector2 Size => Texture == null ? new Vector2() : new Vector2(Texture.Width, Texture.Height);
        public Texture2D Texture 
        { 
            get => _texture;
            set
            {
                _texture = value;
               if (_transform != null)
               {
                    _transform.Origin = Center;
               }
            } 
        }
        private Texture2D _texture;
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
                spriteBatch.Draw(_texture, _transform.Position, null, Color, _transform.Angle, _transform.Origin, Vector2.One, SpriteEffects.None, 1);
            }
        }

        private void Awake()
        {
            _transform.Origin = Center;
        }

        [InsertComponent] private Transform _transform;
    }
}