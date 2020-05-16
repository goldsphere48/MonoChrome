using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.SceneSystem.Layers;
using System;

namespace MonoChrome.SceneSystem
{
    public abstract class Scene : IScene
    {
        public ContentManager Content { get; private set; }
        public Game Game { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        protected LayerManager LayerManager { get; private set; }
        internal event EventHandler<AddGameObjectEventArgs> Added;
        internal event EventHandler<RemoveGameObjectEventArgs> Drop;

        public void Destroy(GameObject gameObject)
        {
            Drop?.Invoke(this, new RemoveGameObjectEventArgs(gameObject));
        }

        public void Instatiate(GameObject gameObject, string layerName)
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

        public void Instatiate(GameObject gameObject, DefaultLayers layerName)
        {
            Instatiate(gameObject, layerName.ToString());
        }

        public void Instatiate(GameObject gameObject)
        {
            Instatiate(gameObject, DefaultLayers.Default.ToString());
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnEnable()
        {
        }

        public abstract void Setup();

        internal void Initialize(LayerManager layerManager, ContentManager content, GraphicsDevice device, Game game)
        {
            LayerManager = layerManager;
            Content = content;
            GraphicsDevice = device;
            Game = game;
        }
    }

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
}