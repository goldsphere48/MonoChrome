using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Input
{
    public abstract class InputListener : IMouseClickHandler
    {
        private MouseState oldState = Mouse.GetState();

        public void HandleMouseClick()
        {
            MouseState newState = Mouse.GetState();
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                OnMouseClick(new PointerEventData { Button = MouseButton.Left, Position = newState.Position.ToVector2() });
            }
            if (newState.MiddleButton == ButtonState.Released && oldState.MiddleButton == ButtonState.Pressed)
            {
                OnMouseClick(new PointerEventData { Button = MouseButton.Middle, Position = newState.Position.ToVector2() });
            }
            if (newState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                OnMouseClick(new PointerEventData { Button = MouseButton.Right, Position = newState.Position.ToVector2() });
            }
            oldState = newState;
        }

        public abstract void OnMouseClick(PointerEventData pointerEventData);
    }
}
