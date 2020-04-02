using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Scenes
{
    /// <summary>
    /// Add additional functionality to work with SceneManager
    /// </summary>
    class SceneController : IDisposable
    {
        private Scene _scene;

        public SceneController(Scene scene, GraphicsDevice device)
        {
            _scene = scene;
            if (_scene.SpriteBatch == null)
            {
                _scene.SpriteBatch = new SpriteBatch(device);
            }
        }

        #region Scene Interface
        public void Draw(GameTime gameTime)
        {
            _scene.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _scene.Update(gameTime);
        }
        #endregion

        #region Scene Controller Interface
        public bool IsInitialized { get; set; } = false;
        public bool IsDisposed { get; set; } = false;

        public void Enable()
        {
            _scene.OnEnable();
        }
        public void Disable()
        {
            _scene.OnDisable();
        }
        public void Initialize()
        {
            IsInitialized = true;
            IsDisposed = false;
            _scene.Awake();
        }
        public void CleanUp(bool clean)
        {
            if (!IsDisposed)
            {
                if (clean)
                {
                    _scene.CleanControlled();
                }
                _scene.Finalise();
                IsDisposed = true;
                IsInitialized = false;
            }
        }
        #endregion

        public void Dispose()
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
