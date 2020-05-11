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
        private Transform _transform;
        private Rectangle _box;
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
            if (IsUseRendererBounds)
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
            CalculateBounds();
            IsUseRendererBounds = true;
        }

        public void UseCustomBounds(int width, int height)
        {
            _box = new Rectangle(0, 0, width, height);
            IsUseRendererBounds = false;
        }

        internal override void DrawBounds(SpriteBatch batch)
        {
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, 2, _box.Height), Color.Black); // Left
            batch.Draw(_debugTexture, new Rectangle(_box.Right, _box.Top, 2, _box.Height), Color.Black); // Right
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, _box.Width, 2), Color.Black); // Top
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Bottom, _box.Width + 2, 2), Color.Black); // Bottom
        }

        private void CalculateBounds()
        {
            var renderers = GameObject.GetComponentsInChildren(typeof(Renderer), true);
            var left = int.MaxValue;
            var top = int.MaxValue;
            var width = 0;
            var height = 0;
            foreach (Renderer renderer in renderers)
            {
                if (renderer is DebugRenderer)
                {
                    continue;
                }
                var size = renderer.Size;
                var collider = renderer.GetComponent<Collider>();
                if (collider != null && !collider.IsUseRendererBounds && collider is BoxCollider2D boxCollider)
                {
                    size = new Vector2(boxCollider._box.Width, boxCollider._box.Width);
                }
                var position = renderer.Transform.Position;

                left = (int)(position.X < left ? position.X : left);
                top = (int)(position.Y < top ? position.Y : top);
                int possibleWidth = (int)(position.X + size.X - left);
                width = possibleWidth > width ? possibleWidth : width;
                int possibleHeight = (int)(position.Y + size.Y - top);
                height = possibleHeight > width ? possibleHeight : height;
            }
            _box = new Rectangle(left, top, width, height);
        }

        private void OnFinalise()
        {
            if (_debugTexture != null)
            {
                _debugTexture.Dispose();
                _debugTexture = null;
            }
        }

        private void Update()
        {
            _box.X = (int)_transform.Position.X;
            _box.Y = (int)_transform.Position.Y;
        }
    }
}
