using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Input
{
    public interface IKeyboardHandler
    {
        void KeyboardHandle(KeyboardState state);
    }
}
