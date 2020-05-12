using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Input;
using MonoChrome.SceneSystem.Layers;
using System;

namespace MonoChrome.SceneSystem
{
    internal class AddGameObjectEventArgs : EventArgs 
    {
        public GameObject GameObject { get; }
        public string LayerName { get; }
        public AddGameObjectEventArgs(GameObject gameObject, string layerName)
        {
            GameObject = gameObject;
            LayerName = layerName;
        }
    }

    internal class RemoveGameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; }
        public RemoveGameObjectEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }

    public abstract class Scene : IScene
    {
        internal event EventHandler<AddGameObjectEventArgs> Added;
        internal event EventHandler<RemoveGameObjectEventArgs> Drop;

        protected LayerManager LayerManager { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Game Game { get; private set; }

        internal void Initialize(LayerManager layerManager, ContentManager content, GraphicsDevice device, Game game)
        {
            LayerManager = layerManager;
            Content = content;
            GraphicsDevice = device;
            Game = game;
        }

        public void Add(GameObject gameObject, string layerName)
        {
            if (gameObject == null || layerName == null)
            {
                throw new ArgumentNullException();
            }
            gameObject.Scene = this;
            gameObject.Awake();
            Added?.Invoke(this, new AddGameObjectEventArgs(gameObject, layerName));
            gameObject.Start();
        }

        public void Add(GameObject gameObject, DefaultLayers layerName)
        {
            Add(gameObject, layerName.ToString());
        }

        public void Add(GameObject gameObject)
        {
            Add(gameObject, DefaultLayers.Default.ToString());
        }
        public void Remove(GameObject gameObject)
        {
            Drop?.Invoke(this, new RemoveGameObjectEventArgs(gameObject));
            gameObject.Dispose();
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
