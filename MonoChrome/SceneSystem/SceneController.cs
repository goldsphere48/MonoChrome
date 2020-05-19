using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Input;
using MonoChrome.SceneSystem.Layers;
using System;

namespace MonoChrome.SceneSystem
{
    internal class SceneController : InputListener, IScene, IDisposable
    {
        public bool Disposed { get; private set; } = false;
        public bool Initialized { get; private set; } = false;
        public Type SceneType => _scene.GetType();

        public SceneController(Type sceneType, GraphicsDevice device, ContentManager content, Game game)
        {
            _store = new EntityStore();
            _layerManager = new LayerManager();
            _spriteBatch = new SpriteBatch(device);
            Entity.Registry = _store;
            _scene = CreateScene(sceneType);
            _scene.Initialize(_layerManager, content, device, game);
            _scene.Added += OnAdd;
            _scene.Drop += OnRemove;
        }

        ~SceneController()
        {
            Dispose(false);
        }

        private LayerManager _layerManager;
        private Scene _scene;
        private SpriteBatch _spriteBatch;
        private EntityStore _store;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _layerManager.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void KeyboardHandle(KeyboardState state)
        {
            _layerManager.KeyboardHandle(state);
        }

        public void OnDisable()
        {
            _scene.OnDisable();
        }

        public void OnEnable()
        {
            Entity.Registry = _store;
            _scene.OnEnable();
        }

        public override void OnMouseClick(PointerEventData pointerEventData)
        {
            _layerManager.HandleMouseClick(pointerEventData);
        }

        public override void OnMouseMove(PointerEventData pointerEventData)
        {
            _layerManager.HandleMouseMove(pointerEventData);
        }

        public void Setup()
        {
            Entity.Registry = _store;
            _layerManager.Initialize();
            _scene.Setup();
            Initialized = true;
            Disposed = false;
        }

        public void Update()
        {
            _layerManager.OnFrameBegin();
            HandleInput();
            _layerManager.Update();
            _layerManager.OnFrameEnd();
        }

        internal Scene GetScene()
        {
            return _scene;
        }

        private Scene CreateScene(Type type)
        {
            return Activator.CreateInstance(type) as Scene;
        }

        private void Dispose(bool clean)
        {
            _layerManager.AddFrameEndTask(() =>
            {
                OnDisable();
                if (Disposed == false)
                {
                    if (clean)
                    {
                        OnDestroy();
                    }
                    OnFinalize();
                    Disposed = true;
                    Initialized = false;
                }
            });
        }

        private void OnAdd(object sender, AddGameObjectEventArgs args)
        {
            _layerManager.Add(args.LayerName, args.GameObject);
        }

        private void OnDestroy()
        {
            _layerManager.OnDestroy();
        }

        private void OnFinalize()
        {
            _layerManager.OnFinalize();
            _layerManager.Clear();
            _store.Clear();
        }

        private void OnRemove(object sender, RemoveGameObjectEventArgs args)
        {
            _layerManager.Remove(args.GameObject);
        }
    }
}