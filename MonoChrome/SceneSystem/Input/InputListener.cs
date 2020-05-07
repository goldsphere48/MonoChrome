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

        public void HandleInput()
        {
            MouseState newState = Mouse.GetState();
            MouseButton button = MouseButton.Left;
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                button = MouseButton.Left;
            }
            if (newState.MiddleButton == ButtonState.Released && oldState.MiddleButton == ButtonState.Pressed)
            {
                button = MouseButton.Middle;
            }
            if (newState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                button = MouseButton.Right;
            }
            OnMouseClick(new PointerEventData { Button = button, Position = newState.Position.ToVector2()});
            oldState = newState;
        }

        public abstract void OnMouseClick(PointerEventData pointerEventData);
    }
}
