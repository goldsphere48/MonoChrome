using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    abstract class Screen : IScreen, IScreenController, IDisposable
    {
        private bool _initialized = false;
        private bool _disposed = false;
        private bool _enabled = false;

        public static readonly ScreenManager ScreenManager = new StackScreenManager();

        #region Interface

        public virtual void Awake()
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {

        }

        public virtual void CleanControlled()
        {

        }

        public virtual void Finalise()
        {

        }

        #endregion

        #region Screen Controller
        bool IScreenController.IsInitialized => _initialized;
        bool IScreenController.IsDisposed => _disposed;
        bool IScreenController.IsEnabled => _enabled;

        void IScreenController.Enable()
        {
            _enabled = true;
            OnEnable();
        }
        void IScreenController.Disable()
        {
            _enabled = false;
            OnDisable();
        }
        void IScreenController.Initialize()
        {
            _initialized = true;
            _disposed = false;
            _enabled = true;
            Awake();
        }
        void IScreenController.CleanUp(bool clean)
        {
            if (!_disposed)
            {
                if (clean)
                {
                    CleanControlled();
                }
                Finalise();
                _disposed = true;
                _initialized = false;
            }
        }
        #endregion

        public void Dispose()
        {
            (this as IScreenController).CleanUp(true);
            GC.SuppressFinalize(true);
        }

        ~Screen()
        {
            (this as IScreenController).CleanUp(false);
        }

    }
}
