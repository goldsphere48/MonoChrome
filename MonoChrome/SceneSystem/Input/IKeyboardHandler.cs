using Microsoft.Xna.Framework.Input;

namespace MonoChrome.SceneSystem.Input
{
    public interface IKeyboardHandler
    {
        void KeyboardHandle(KeyboardState state);
    }
}