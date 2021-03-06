﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoChrome.Core.Components
{
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; set; }

        public GameObjectEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }

    public class Transform : Component
    {
        public float Angle { get; set; }
        public List<Transform> Childrens { get; } = new List<Transform>();
        public Vector2 LocalPosition
        {
            get
            {
                if (Parent != null)
                {
                    return _position - Parent.Position;
                }
                return _position;
            }
            set => HandleLocalPositionChange(value);
        }
        public Vector2 Origin { get; set; }
        public Transform Parent
        {
            get => _parent;
            set
            {
                var args = new GameObjectEventArgs(GameObject);
                if (value != null && value.Childrens.Contains(this) == false)
                {
                    value.Childrens.Add(this);
                    ChildAdded?.Invoke(this, args);
                    OnChildAdded(args);
                   
                }
                else if (value == null && _parent == null)
                {
                    ChildRemove?.Invoke(this, args);
                    OnChildRemove(args);
                    _parent.Childrens.Remove(this);
                }
                _parent = value;
            }
        }
        public Vector2 Position { get => _position; set => HandleAbsolutePositionChange(value); }
        public event EventHandler<GameObjectEventArgs> ChildAdded;
        public event EventHandler<GameObjectEventArgs> ChildAddedDeep;
        public event EventHandler<GameObjectEventArgs> ChildRemove;
        public event EventHandler<GameObjectEventArgs> ChildRemoveDeep;
        private Transform _parent;
        private Vector2 _position = Vector2.Zero;

        public void MoveTowards(Vector2 dir, Vector2 speed)
        {
            var newVector = new Vector2(dir.X - Position.X, dir.Y - Position.Y);
            newVector.Normalize();
            newVector *= speed;
            Position += newVector;
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

        private void HandleLocalPositionChange(Vector2 newLocalPosition)
        {
            if (Parent != null)
            {
                Position = newLocalPosition + Parent.Position;
            }
            else
            {
                Position = newLocalPosition;
            }
        }

        private void OnChildAdded(GameObjectEventArgs args)
        {
            if (Parent != null)
            {
                Parent.OnChildAdded(args);
            }
            ChildAddedDeep?.Invoke(this, args);
        }

        private void OnChildRemove(GameObjectEventArgs args)
        {
            if (Parent != null)
            {
                Parent.OnChildRemove(args);
            }
            ChildRemoveDeep?.Invoke(this, args);
        }
    }
}