using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoChrome.Core;
using MonoChrome.Core.Attributes;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public class BoxCollider2D : Collider
    {
        [InsertComponent]
        private Transform transform;
        private Rectangle _box;
        private bool _useRendererBounds = true;
        private Texture2D _debugTexture;

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

        public void Awake()
        {
            if (_useRendererBounds)
            {
                UseRendererBounds();
            }
            if (_debugTexture == null)
            {
                _debugTexture = new Texture2D(GameObject.Scene.GraphicsDevice, 1, 1);
                _debugTexture.SetData(new[] { Color.Green });
            }
        }

        public BoxCollider2D()
        {

        }

        public BoxCollider2D(int width, int height)
        {
            UseCustomBounds(width, height);
        }

        public void UseRendererBounds()
        {
            _box = new Rectangle();
            var renderer = this.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                _box = new Rectangle(0, 0, (int)renderer.Size.X, (int)renderer.Size.Y);
            }
            else
            {
                _box = new Rectangle(0, 0, 0, 0);
            }
            _useRendererBounds = true;
        }

        public void UseCustomBounds(int width, int height)
        {
            _box = new Rectangle(0, 0, width, height);
            _useRendererBounds = false;
        }

        internal override void DrawBounds(SpriteBatch batch)
        {
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, 2, _box.Height), Color.Black); // Left
            batch.Draw(_debugTexture, new Rectangle(_box.Right, _box.Top, 2, _box.Height), Color.Black); // Right
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, _box.Width, 2), Color.Black); // Top
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Bottom, _box.Width + 2, 2), Color.Black); // Bottom
        }

        private void Update()
        {
            _box.X = (int)transform.Position.X;
            _box.Y = (int)transform.Position.Y;
        }

        private void OnFinalise()
        {
            if (_debugTexture != null)
            {
                _debugTexture.Dispose();
                _debugTexture = null;
            }
        }
    }
}
