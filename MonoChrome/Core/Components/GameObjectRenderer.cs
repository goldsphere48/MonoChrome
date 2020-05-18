using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using System;

namespace MonoChrome.Core.Components
{
    public abstract class GameObjectRenderer : Renderer
    {
        protected bool IsVisible { get; set; }
        protected Rectangle Window => new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );
        public event Action<Renderer> BecomeInvisible;
        public event Action<Renderer> BecomeVisible;
        [InsertComponent] private Transform _transform;

        protected void Update()
        {
            var box = new Rectangle((int)_transform.Position.X, (int)_transform.Position.Y, (int)Size.X, (int)Size.Y);
            if (box.Intersects(Window) == false && IsVisible)
            {
                IsVisible = false;
                BecomeInvisible?.Invoke(this);
            }
            else if (IsVisible == false)
            {
                IsVisible = true;
                BecomeVisible?.Invoke(this);
            }
        }
    }
}