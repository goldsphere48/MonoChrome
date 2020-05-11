using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.Core.Helpers;
using System;
using MonoChrome.SceneSystem.Layers;
using MonoChrome.SceneSystem.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoChrome.SceneSystem
{
    internal class SceneController : InputListener, IScene, IDisposable
    {
        public bool Initialized { get; private set; } = false;
        public bool Disposed { get; private set; } = false;
        public Type SceneType => _scene.GetType();

        private Scene _scene;
        private SpriteBatch _spriteBatch;
        private EntityStore _store;
        private LayerManager _layerManager;

        public SceneController(Type sceneType, GraphicsDevice device, ContentManager content)
        {
            _store = new EntityStore();
            _layerManager = new LayerManager();
            _spriteBatch = new SpriteBatch(device);
            Entity.Registry = _store;
            _scene = CreateScene(sceneType);
            _scene.Initialize(_layerManager, content, device);
            _scene.Added += OnAdd;
            _scene.Drop += OnRemove;
        }

        private void OnAdd(object sender, AddGameObjectEventArgs args)
        {
            _layerManager.Add(args.LayerName, args.GameObject);
        }
        private void OnRemove(object sender, RemoveGameObjectEventArgs args)
        {
            _layerManager.Remove(args.GameObject);
        }

        #region Scene Interface
        public void Setup()
        {
            Entity.Registry = _store;
            _scene.Setup();
            Initialized = true;
            Disposed = false;
        }

        public void OnEnable()
        {
            Entity.Registry = _store;
            _scene.OnEnable();
        }

        public void OnDisable()
        {
            _scene.OnDisable();
        }
        #endregion

        #region Scene Controller Interface
        public void Update()
        {
            HandleMouseEvents();
            _layerManager.Update();
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _layerManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        #endregion

        private Scene CreateScene(Type type)
        {
            return Activator.CreateInstance(type) as Scene;
        }

        #region Disposable
        private void Dispose(bool clean)
        {
            OnDisable();
            if (!Disposed)
            {
                if (clean)
                {
                    OnDestroy();
                }
                OnFinalize();
                Disposed = true;
                Initialized = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        private void OnDestroy()
        {
            _layerManager.OnDestroy();
        }

        private void OnFinalize()
        {
            _layerManager.OnFinalise();
            _layerManager.Clear();
            _store.Clear();
        }

        public override void OnMouseClick(PointerEventData pointerEventData)
        {
            _layerManager.HandleMouseClick(pointerEventData);
        }

        public override void OnMouseMove(PointerEventData pointerEventData)
        {
            _layerManager.HandleMouseMove(pointerEventData);
        }

        ~SceneController()
        {
            Dispose(false);
        }
        #endregion
    }
}
