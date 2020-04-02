using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Actors
{
    class Group : Node
    {
        private List<Node> _childrens = new List<Node>();

        public void Add(Node gameObject)
        {
            Debug.Assert(gameObject != null);
            _childrens.Add(gameObject);
        }


        public void Remove(Node gameObject)
        {
            Debug.Assert(gameObject != null);
            _childrens.Remove(gameObject);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (var child in _childrens)
            {
                child.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var child in _childrens)
            {
                child.Update(gameTime);
            }
        }

        public override void OnEnable()
        {
            foreach (var child in _childrens)
            {
                child.OnEnable();
            }
        }

        public override void OnDisable()
        {
            foreach (var child in _childrens)
            {
                child.OnDisable();
            }
        }

        public override void OnDestroy()
        {
            foreach (var child in _childrens)
            {
                child.OnDestroy();
            }
        }
    }
}
