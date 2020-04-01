using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    class StackScreenManager : ScreenManager
    {
        private static Stack<Screen> _screens = new Stack<Screen>();

        public override void SetActive(Screen screen)
        {
            if (screen == null)
            {
                throw new Exception("Screen have to be not null");
            }

            _screens.Push(screen);
            var screenController = _currentScreen as IScreenController;
            screenController?.Disable();

            if (_currentScreen == screen)
            {
                return;
            }

            _currentScreen = screen;
            if (!screenController.IsInitialized)
            {
                screenController.Initialize();
            }
            screenController.Enable();
        }

        public override void Replace(Screen screen)
        {
            _screens.Pop();
            SetActive(screen);
        }

        public override void GoBack()
        {
            var screenController = _currentScreen as IScreenController;
            screenController?.Disable();
            _currentScreen = _screens.Pop();
            screenController?.Enable();
        }

        public override void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            _currentScreen?.Draw(gameTime, batch);
        }
    }
}
