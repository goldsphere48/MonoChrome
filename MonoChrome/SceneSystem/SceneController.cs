using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoChrome.SceneSystem
{
    /// <summary>
    /// Add additional functionality to work with SceneManager
    /// </summary>
    internal class SceneController : IScene, IDisposable
    {
        public bool Initialized { get; private set; } = false;
        public bool Disposed { get; private set; } = false;
        public Type SceneType => GetType();

        private Scene _scene;
        private SpriteBatch _spriteBatch;

        public SceneController(Scene scene, GraphicsDevice device)
        {
            _scene = scene;
            _spriteBatch = new SpriteBatch(device);
        }

        #region Scene Interface
        public void Setup()
        {
            _scene.Setup();
            Initialized = true;
            Disposed = false;
        }

        public void OnEnable()
        {
            _scene.OnEnable();
        }

        public void OnDisable()
        {
            _scene.OnDisable();
        }

        public void OnDestroy()
        {
            _scene.OnDestroy();
        }

        public void OnFinalize()
        {
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

        public void CleanUp(bool clean)
        {
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
        #endregion

        void IDisposable.Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(true);
        }

        ~SceneController()
        {
            CleanUp(false);
        }
    }
}
