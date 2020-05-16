using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components
{
    public abstract class GameObjectRenderer : Renderer
    {
        public event Action<Renderer> BecomeInvisible;
        public event Action<Renderer> BecomeVisible;
        protected Rectangle Window => new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );
        protected bool IsVisible { get; set; }

        protected void Update()
        {
            var box = new Rectangle((int)_transform.Position.X, (int)_transform.Position.Y, (int)Size.X, (int)Size.Y);
            if (!box.Intersects(Window) && IsVisible)
            {
                IsVisible = false;
                BecomeInvisible?.Invoke(this);
            }
            else if (!IsVisible)
            {
                IsVisible = true;
                BecomeVisible?.Invoke(this);
            }
        }
        [InsertComponent] private Transform _transform;
    }
}
