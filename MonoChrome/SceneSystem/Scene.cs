using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using System;

namespace MonoChrome.SceneSystem
{
    public abstract class Scene : IScene
    {
        public GameObject Root { get; } = Entity.Create("Root");
        public Type GetSceneType => GetType();

        public void Add(GameObject gameObject)
        {
            Root.Transform.Parent = gameObject.Transform;
        }
        public void Remove(GameObject gameObject)
        {
            Root.Transform.Parent = null;
        }

        public virtual void OnDisable()
        {

        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDestroy()
        {

        }
        public virtual void OnFinalize()
        {

        }
        public abstract void Setup();
    }
}
