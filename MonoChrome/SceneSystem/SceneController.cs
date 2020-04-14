using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.EntityManager;
using System;
using System.Linq;

namespace MonoChrome.SceneSystem
{
    internal class SceneController : IScene, IDisposable
    {
        public bool Initialized { get; private set; } = false;
        public bool Disposed { get; private set; } = false;
        public Type SceneType => _scene.SceneType;

        private IScene _scene;
        private SpriteBatch _spriteBatch;
        private EntityStore _store = new EntityStore();
        private CachedComponents _cachedComponents = new CachedComponents();
        private CachedMethods _cachedMethods = new CachedMethods();
        private Type _rendererType = typeof(Renderer2D);

        public SceneController(IScene scene, GraphicsDevice device)
        {
            _scene = scene;
            _spriteBatch = new SpriteBatch(device);
        }

        #region Store Events
        private void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            if (componentArgs.Component is Renderer2D)
            {
                _cachedComponents.Cache(componentArgs.GetType(), componentArgs.Component);
            }
            CacheMethod("Update", componentArgs.Component.UpdateMethod);
            CacheMethod("OnDisable", componentArgs.Component.OnDisableMethod);
            CacheMethod("OnDestroy", componentArgs.Component.OnDestroyMethod);
            CacheMethod("OnEnable", componentArgs.Component.OnEnableMethod);
            CacheMethod("OnFinalise", componentArgs.Component.OnFinaliseMethod);
        }
        private void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            if (componentArgs.Component is Renderer2D)
            {
                _cachedComponents.Remove(componentArgs.Component.GetType());
            }
        }
        private void CacheMethod(string methodName, Action method)
        {
            if (method != null)
            {
                _cachedMethods.Cache(methodName, method);
            }
        }
        #endregion

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
            _cachedMethods["OnEnable"]();
            _scene.OnEnable();
        }

        public void OnDisable()
        {
            _cachedMethods["OnDisable"]();
            _scene.OnDisable();
        }

        public void OnDestroy()
        {
            _cachedMethods["OnDestroy"]();
            _scene.OnDestroy();
        }

        public void OnFinalize()
        {
            _cachedMethods["OnFinalize"]();
            //Entity.Registry.Clear();
            _scene.OnFinalize();
        }
        #endregion

        #region Scene Controller Interface
        public void Update()
        {
            _cachedMethods["Update"]();
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            foreach (Renderer2D renderer in _cachedComponents[_rendererType])
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
