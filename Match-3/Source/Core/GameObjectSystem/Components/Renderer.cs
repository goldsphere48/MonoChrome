using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Core.GameObjectSystem.Components
{
    abstract class Renderer : Component
    {
        public abstract void Draw(GameTime gameTime);
    }
}
