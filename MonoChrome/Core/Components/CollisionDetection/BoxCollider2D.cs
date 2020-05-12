using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.Attributes;
using System.Collections.Generic;

namespace MonoChrome.Core.Components.CollisionDetection
{
    public class BoxCollider2D : Collider
    {
        public Rectangle Box => _box;
        public BoxCollider2D()
        {
        }
        public BoxCollider2D(int width, int height)
        {
            UseCustomBounds(width, height);
        }
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
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, 2, _box.Height), Color.Black); // Left
            batch.Draw(_debugTexture, new Rectangle(_box.Right, _box.Top, 2, _box.Height), Color.Black); // Right
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Top, _box.Width, 2), Color.Black); // Top
            batch.Draw(_debugTexture, new Rectangle(_box.Left, _box.Bottom, _box.Width + 2, 2), Color.Black); // Bottom
        }
        private Rectangle _box;
        private IList<BoxCollider2D> _childColliders = new List<BoxCollider2D>();
        private Texture2D _debugTexture;
        [InsertComponent(Required = true, Inherit = true)] private Renderer _renderer;
        [InsertComponent] private Transform _transform;
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
            _box.X = (int)(_transform.Position.X);
            _box.Y = (int)(_transform.Position.Y);
        }
    }
}