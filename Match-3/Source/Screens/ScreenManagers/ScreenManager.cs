using Match_3.Source.Screens.ScreenTransitionEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    abstract class ScreenManager
    {
        protected Screen _currentScreen;

        public abstract void SetActive(Screen screen);

        public virtual void Replace(Screen screen) => SetActive(screen);

        public abstract void GoBack();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch batch);
    }
}
