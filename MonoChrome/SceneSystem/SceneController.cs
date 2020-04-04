using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoChrome.Core.SceneSystem
{
    /// <summary>
    /// Add additional functionality to work with SceneManager
    /// </summary>
    internal class SceneController : IDisposable
    {
        public Scene Scene { get; }

        public SceneController(Scene scene, GraphicsDevice device)
        {
            Scene = scene;
            if (Scene.SpriteBatch == null)
            {
                Scene.SpriteBatch = new SpriteBatch(device);
            }
        }

        #region Scene Interface
        public void Draw()
        {
            //Scene.Draw();
        }

        public void Update()
        {
            Scene.Update();
        }
        #endregion

        #region Scene Controller Interface
        public bool IsInitialized { get; private set; } = false;
        public bool IsDisposed { get; private set; } = false;

        public void Enable()
        {
            Scene.Enabled = true;
        }
        public void Disable()
        {
            Scene.Enabled = false;
        }
        public void Initialize()
        {
            IsInitialized = true;
            IsDisposed = false;
            Scene.Awake();
        }
        public void CleanUp(bool clean)
        {
            if (!IsDisposed)
            {
                if (clean)
                {
                    Scene.OnDestroy();
                }
                Scene.OnFinalize();
                IsDisposed = true;
                IsInitialized = false;
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
