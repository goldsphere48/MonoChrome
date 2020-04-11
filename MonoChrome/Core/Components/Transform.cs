using Microsoft.Xna.Framework;
using MonoChrome.Core.Attributes;
using System;
using System.Collections.Generic;

namespace MonoChrome.Core.Components
{
    [ComponentUsage(AllowMultipleComponentUsage = false)]
    public class Transform : Component
    {
        private Vector2 _position = Vector2.Zero;
        private Transform _parent;

        public List<Transform> Childrens { get; } = new List<Transform>();
        public Transform Parent 
        { 
            get => _parent;
            set
            {
                if (value == null)
                {
                    if (_parent != null)
                    {
                        _parent.Childrens.Remove(this);
                    }
                } else if (!value.Childrens.Contains(this))
                {
                    value.Childrens.Add(this);
                }
                _parent = value;
            }
        }
        public Vector2 LocalPosition 
        {
            get 
            { 
                if (Parent == null)
                {
                    return _position;
                }
                return _position - Parent.Position;
            }
            set => HandleLocalPositionChange(value);
        }
        public Vector2 Position
        {
            get => _position;
            set => HandleAbsolutePositionChange(value);
        }

        private void HandleLocalPositionChange(Vector2 newLocalPosition)
        {
            if (Parent != null)
            {
                Position = newLocalPosition + Parent.Position;
            } else
            {
                Position = newLocalPosition;
            }
        }

        private void HandleAbsolutePositionChange(Vector2 newAbsolutePosition)
        {
            if (newAbsolutePosition != _position)
            {
                var dv = newAbsolutePosition - _position;
                _position = newAbsolutePosition;
                foreach (var child in Childrens)
                {
                    child.Position += dv;
                }
            }
        }
    }
}
