using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens.ScreenTransitionEffects
{
    abstract class ScreenTransitionEffect
    {
        protected bool _finished = true;

        public event Action OnFinished;

        public bool Finished => _finished;

        public virtual void Draw(GameTime gameTime, SpriteBatch batch) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Reset() { }
        public virtual void Start() 
        {
            _finished = false;
        }
        public virtual void Finish() 
        {
            _finished = true;
            OnFinished?.Invoke();
        }
    }
}
