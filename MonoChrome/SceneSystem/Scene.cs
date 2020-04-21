﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;

namespace MonoChrome.SceneSystem
{
    public abstract class Scene : IScene
    {
        public Type SceneType => GetType();
        
        protected GameObject Root { get => _root; }
        
        private GameObject _root;

        public void Add(GameObject gameObject)
        {
            Root.Transform.Parent = gameObject.Transform;
            gameObject.Awake();
        }
        public void Remove(GameObject gameObject)
        {
            Root.Transform.Parent = null;
            gameObject.Dispose();
        }

        public virtual void OnDisable()
        {

        }
        public virtual void OnEnable()
        {

        }
        public virtual void Setup()
        {
            _root = Entity.Create("Root");
        }
    }
}
