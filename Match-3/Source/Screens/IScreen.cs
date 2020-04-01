using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    interface IScreen
    {

        void Awake();
        void OnEnable();
        void OnDisable();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch batch);
        void CleanControlled();
        void Finalise();
    }
}
