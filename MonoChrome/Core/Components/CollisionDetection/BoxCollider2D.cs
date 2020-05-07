using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoChrome.Core;
using MonoChrome.Core.Attributes;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public class BoxCollider2D : Collider
    {
        [InsertComponent]
        private Transform transform;

        private Rectangle _box; 
        public override void CheckCollisionWith(Collider collider)
        {
            if (collider is BoxCollider2D boxCollider)
            {
                if (boxCollider._box.Intersects(_box))
                {
                    var collision = new Collision(collider.GameObject);
                    var components = this.GetComponents();
                    foreach (var component in components)
                    {
                        component.OnCollision?.Invoke(collision);
                    }
                }
            }
        }

        public override bool Contains(Vector2 point)
        {
            return _box.Contains(point.ToPoint());
        }

        public BoxCollider2D()
        {
            _box = new Rectangle();
            var renderer = this.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                _box = new Rectangle(0, 0, (int)renderer.Size.X, (int)renderer.Size.Y);
            } else
            {
                _box = new Rectangle(0, 0, 0, 0);
            }
        }

        public BoxCollider2D(int width, int height)
        {
            _box = new Rectangle(0, 0, width, height);
        }

        private void Update()
        {
            _box.X = (int)transform.Position.X;
            _box.Y = (int)transform.Position.Y;
        }
    }
}
