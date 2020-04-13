using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.EntityManager;
using System;

namespace MonoChrome.SceneSystem
{
    internal class SceneController : IScene, IDisposable
    {
        public bool Initialized { get; private set; } = false;
        public bool Disposed { get; private set; } = false;
        public Type SceneType => _scene.SceneType;

        private IScene _scene;
        private SpriteBatch _spriteBatch;
        private EntityRegistry _registry = new EntityRegistry();
        private Type _rendererType = typeof(Renderer2D);

        public SceneController(IScene scene, GraphicsDevice device)
        {
            //Entity.Registry.CreateCOntext(this);
            _scene = scene;
            _spriteBatch = new SpriteBatch(device);
        }

        #region Scene Interface
        public void Setup()
        {
            Entity.Registry = _registry;
            _scene.Setup();
            Initialized = true;
            Disposed = false;
        }

        public void OnEnable()
        {
            Entity.Registry = _registry;
            _scene.OnEnable();
        }

        public void OnDisable()
        {
            _scene.OnDisable();
        }

        public void OnDestroy()
        {
            //Entity.Registry.OnDestroy();
            _scene.OnDestroy();
        }

        public void OnFinalize()
        {
            //Entity.Registry.OnFinalize();
            //Entity.Registry.Clear();
            _scene.OnFinalize();
        }
        #endregion

        #region Scene Controller Interface
        public void Update()
        {
            // Вызывать все методы Update текущего контекста
            var components = _registry.Store.GetComponents<Component>(true);
            foreach (var component in components)
            {
                component.Update();
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            // Вызывать все методы Draw, из Renderer'ов текущего контекста
            foreach (Renderer renderer in _registry.CachedComponents[_rendererType])
            {
                renderer.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }
        #endregion

        #region Disposable
        public void CleanUp(bool clean)
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
            CleanUp(true);
            GC.SuppressFinalize(true);
        }

        ~SceneController()
        {
            CleanUp(false);
        }
        #endregion
    }
}
