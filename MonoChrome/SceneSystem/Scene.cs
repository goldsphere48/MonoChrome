using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using System;

namespace MonoChrome.SceneSystem
{
    public abstract class Scene : IScene
    {
        public GameObject Root => _root;

        private GameObject _root = new GameObject();

        public Type GetSceneType => GetType();

        public void Add(GameObject gameObject)
        {
            _root.Transform.Parent = gameObject.Transform;
        }
        public void Remove(GameObject gameObject)
        {
            _root.Transform.Parent = null;
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
