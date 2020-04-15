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
            _store.ComponentAdded += OnComponentAdded;
            _store.ComponentRemoved += OnComponentRemoved;
            _store.ComponentEnabled += OnComponentEnabled;
            _store.ComponentDisabled += OnComponentDisabled;
        }

        #region Store Events
        private void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            CacheComponent(typeof(Renderer2D), componentArgs.Component);
            CacheMethod("Update", componentArgs.Component.UpdateMethod);
            CacheMethod("OnDestroy", componentArgs.Component.OnDestroyMethod);
            CacheMethod("OnFinalise", componentArgs.Component.OnFinaliseMethod);
        }
        private void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            if (componentArgs.Component is Renderer2D)
            {
                _cachedComponents.Remove(componentArgs.Component.GetType());
            }
        }
        private void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            CacheMethod("Update", componentArgs.Component.UpdateMethod);
            CacheComponent(typeof(Renderer2D), componentArgs.Component);
        }
        private void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            _cachedMethods.Remove("Update", componentArgs.Component.UpdateMethod);
            _cachedComponents.Remove(componentArgs.Component);
        }
        private void CacheMethod(string methodName, Action method)
        {
            if (method != null)
            {
                _cachedMethods.Cache(methodName, method);
            }
        }
        private void CacheComponent(Type cachedType, Component component)
        {
            if (component.GetType() == cachedType)
            {
                _cachedComponents.Cache(cachedType, component);
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
        public void Dispose(bool clean)
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
            _cachedMethods["OnDestroy"]();
        }

        private void OnFinalize()
        {
            _cachedMethods["OnFinalize"]();
            _store.Clear();
            _cachedComponents.Clear();
            _cachedMethods.Clear();
        }

        ~SceneController()
        {
            Dispose(false);
        }
        #endregion
    }
}
