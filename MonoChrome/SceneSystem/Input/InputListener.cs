using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoChrome.SceneSystem.Input
{
    public abstract class InputListener
    {
        public void HandleInput()
        {
            HandleMouseClick();
            HandleMouseMove();
            HandleKeyboardKeyPressed();
        }
        public abstract void OnMouseClick(PointerEventData pointerEventData);
        public abstract void OnMouseMove(PointerEventData pointerEventData);
        public abstract void KeyboardHandle(KeyboardState state);

        private MouseState oldState = Mouse.GetState();
        private Point previousMousePosition;

        private void HandleMouseClick()
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
        private void HandleKeyboardKeyPressed()
        {
            var state = Keyboard.GetState();
            if (state.GetPressedKeys().Length > 0)
            {
                KeyboardHandle(state);
            }
        }
        private void HandleMouseMove()
        {
            MouseState newState = Mouse.GetState();
            if (previousMousePosition != newState.Position)
            {
                OnMouseMove(new PointerEventData { Position = newState.Position.ToVector2() });
            }
            previousMousePosition = newState.Position;
        }
    }
}