using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Match_3.Source.Core.GameObjectSystem.Components
{
    class Transform : Component
    {
        private Vector2 _localPosition = new Vector2(0, 0);
        private Vector2 _position = new Vector2(0, 0);

        public List<Transform> Childrens { get; } = new List<Transform>();
        public Transform Parent { get; set; }
        public Vector2 LocalPosition 
        { 
            get => _localPosition;
            set => HandleLocalPositionChange(value);
        }
        public Vector2 Position
        {
            get => _position;
            set => HandleAbsolutePositionChange(value);
        }

        private void HandleLocalPositionChange(Vector2 newLocalPosition)
        {
            if (newLocalPosition != _localPosition)
            {
                _localPosition = newLocalPosition;
                Position = newLocalPosition + Parent.Position;
            }
        }

        private void HandleAbsolutePositionChange(Vector2 newAbsolutePosition)
        {
            if (newAbsolutePosition != _position)
            {
                var dv = _position - newAbsolutePosition;
                _position = newAbsolutePosition;
                foreach (var child in Childrens)
                {
                    child.Position += dv;
                }
            }
        }
    }
}
