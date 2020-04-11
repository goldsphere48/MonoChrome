using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Match_3.Source.Screens
{
    class DirectScreenManager : ScreenManager
    {
        public DirectScreenManager(ScreenPool pool) : base(pool)
        { 
        
        }

        public override void SetActive(IScreenController screen)
        {
            if (screen == null)
            {
                throw new Exception("Screen must be not null");
            }
            if (_currentScreen == screen)
            {
                return;
            }
            _currentScreen?.Disable();
            _currentScreen = screen;
            InitializeOrEnable(_currentScreen);
        }


        private void InitializeOrEnable(IScreenController screen)
        {
            if (!screen.IsInitialized)
            {
                screen.Initialize();
            }
            else
            {
                screen.Enable();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            _currentScreen?.Draw(gameTime, batch);
        }

        public override void Add(IScreenController screen)
        {
            throw new NotImplementedException();
        }

        public override void Remove(IScreenController screen)
        {
            throw new NotImplementedException();
        }
    }
}
