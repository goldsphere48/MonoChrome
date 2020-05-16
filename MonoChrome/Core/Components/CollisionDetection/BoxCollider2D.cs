using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using System;
using System.Collections.Generic;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public class BoxCollider2D : Collider
    {
        public Rectangle Bounds => _box;

        public BoxCollider2D()
        {
        }

        public BoxCollider2D(int width, int height)
        {
            UseCustomBounds(width, height);
        }

        private Rectangle _box;
        private IList<BoxCollider2D> _childColliders = new List<BoxCollider2D>();
        private Texture2D _debugTexture;
        [InsertComponent(Required = true, Inherit = true)] private Renderer _renderer;
        [InsertComponent] private Transform _transform;

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

        public void UseCustomBounds(int width, int height)
        {
            _box = new Rectangle(0, 0, width, height);
            _transform.Origin = new Vector2(width / 2, height / 2);
            IsUseRendererBounds = false;
        }

        public void UseRendererBounds()
        {
            var renderer = GameObject.GetComponent<Renderer>(true);
            if (renderer != null)
            {
                _box = new Rectangle((int)_transform.Position.X, (int)_transform.Position.Y, (int)renderer.Size.X, (int)renderer.Size.Y);
            }
            else
            {
                _box = new Rectangle();
            }
            IsUseRendererBounds = true;
        }

        internal override void DrawBounds(SpriteBatch batch)
        {
            DrawLine(batch, (new Vector2(_box.Left, _box.Top)), _box.Height, (float)(Math.PI / 2));
            DrawLine(batch, (new Vector2(_box.Right, _box.Top)), _box.Height, (float)(Math.PI / 2));
            DrawLine(batch, (new Vector2(_box.Right, _box.Top)), _box.Width, (float)(-Math.PI));
            DrawLine(batch, (new Vector2(_box.Left, _box.Bottom)), _box.Width, 0);
        }

        private void Awake()
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

        private void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(_debugTexture, point, null, Color.Black, angle, origin, scale, SpriteEffects.None, 0);
        }

        private void OnFinalise()
        {
            if (_debugTexture != null)
            {
                _debugTexture.Dispose();
                _debugTexture = null;
            }
        }

        private Vector2 RotateAroundOrigin(Vector2 point)
        {
            return Vector2.Transform(point - _transform.Position, Matrix.CreateRotationZ(_transform.Angle)) + _transform.Position;
        }

        private void Update()
        {
            _box.X = (int)(_transform.Position.X - _transform.Origin.X);
            _box.Y = (int)(_transform.Position.Y - _transform.Origin.Y);
        }
    }
}