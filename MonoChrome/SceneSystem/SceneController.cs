using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoChrome.SceneSystem
{
    internal class SceneController : IScene, IDisposable
    {
        public bool Initialized { get; private set; } = false;
        public bool Disposed { get; private set; } = false;
        public Type SceneType => GetType();

        private IScene _scene;
        private SpriteBatch _spriteBatch;

        public SceneController(IScene scene, GraphicsDevice device)
        {
            //Entity.Registry.CreateCOntext(this);
            _scene = scene;
            _spriteBatch = new SpriteBatch(device);
        }

        #region Scene Interface
        public void Setup()
        {
            //Entity.Registry.SetContext(this);
            _scene.Setup();
            Initialized = true;
            Disposed = false;
        }

        public void OnEnable()
        {
            //Entity.Registry.SetContext(this);
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
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            // Вызывать все методы Draw, из Renderer'ов текущего контекста
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
