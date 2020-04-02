using Match_3.Source.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Scenes
{
    abstract class Scene
    {
        public readonly SceneId Id;
        public SpriteBatch SpriteBatch { get; set; }

        private Group _root = new Group();

        public Scene(SceneId id)
        {
            Id = id;
        }
        public void Add(Node actor)
        {
            _root.Add(actor);
        }
        public void Remove(Node actor)
        {
            _root.Remove(actor);
        }
        public virtual void Awake()
        {

        }
        public virtual void Draw(GameTime gameTime)
        {
            _root.Draw(gameTime);
        }
        public virtual void Update(GameTime gameTime)
        {
            _root.Update(gameTime);
        }
        public virtual void OnDisable()
        {
            _root.OnDisable();
        }
        public virtual void OnEnable()
        {
            _root.OnEnable();
        }
        public virtual void CleanControlled()
        {

        }
        public virtual void Finalise()
        {

        }
    }
}
