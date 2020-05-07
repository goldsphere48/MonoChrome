using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Input;
using System;

namespace MonoChrome.SceneSystem
{
    public abstract class Scene : InputListener, IScene
    {
        public Type SceneType => GetType();
        protected GameObject Root { get; } = Entity.Create("Root");

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
        public override void OnMouseClick(PointerEventData data)
        {
            foreach (var gameObject in Entity.Registry)
            {
                gameObject.OnMouseClick(data);
            }
        }
        public virtual void OnDisable()
        {

        }
        public virtual void OnEnable()
        {

        }
        public abstract void Setup();
    }
}
