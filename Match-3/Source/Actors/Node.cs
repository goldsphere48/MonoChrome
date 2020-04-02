using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Actors
{
    abstract class Node
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnDestroy();
    }
}
