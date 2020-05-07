using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using MonoChrome.Core.Components;
using MonoChrome.Core.Components.CollisionDetection;
using MonoChrome.Core.EntityManager;
using MonoChrome.Core.Helpers;
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
        private Type _rendererType = typeof(SpriteRenderer);
        private Type _colliderType = typeof(SpriteRenderer);

        public SceneController(Type sceneType, GraphicsDevice device)
        {
            Entity.Registry = _store;
            _scene = CreateScene(sceneType);
            _spriteBatch = new SpriteBatch(device);
            _store.ComponentAdded += OnComponentAdded;
            _store.ComponentRemoved += OnComponentRemoved;
            _store.ComponentEnabled += OnComponentEnabled;
            _store.ComponentDisabled += OnComponentDisabled;
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
            _cachedMethods["Update"]?.Invoke();
            foreach (Collider colliderA in _cachedComponents[_colliderType])
            {
                foreach (Collider colliderB in _cachedComponents[_colliderType])
                {
                    if (colliderA != colliderB)
                    {
                        colliderA.CheckCollisionWith(colliderB);
                    }
                }
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            foreach (SpriteRenderer renderer in _cachedComponents[_rendererType])
            {
                renderer.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }
        #endregion

        private IScene CreateScene(Type type)
        {
            return Activator.CreateInstance(type) as IScene;
        }

        #region Store Events
        private void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            CacheComponent(typeof(SpriteRenderer), componentArgs.Component);
            CacheComponent(typeof(BoxCollider2D), componentArgs.Component);
            CacheMethod("Update", componentArgs.Component.UpdateMethod);
            CacheMethod("OnDestroy", componentArgs.Component.OnDestroyMethod);
            CacheMethod("OnFinalise", componentArgs.Component.OnFinaliseMethod);
        }
        private void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            if (componentArgs.Component is SpriteRenderer)
            {
                _cachedComponents.Remove(componentArgs.Component.GetType());
            }
            if (componentArgs.Component is BoxCollider2D)
            {
                _cachedComponents.Remove(componentArgs.Component.GetType());
            }
            _cachedMethods.Remove("Update", componentArgs.Component.UpdateMethod);
            _cachedMethods.Remove("OnDestroy", componentArgs.Component.OnDestroyMethod);
            _cachedMethods.Remove("OnFinalise", componentArgs.Component.OnFinaliseMethod);
        }
        private void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            CacheMethod("Update", componentArgs.Component.UpdateMethod);
            CacheComponent(typeof(SpriteRenderer), componentArgs.Component);
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
            _cachedMethods["OnDestroy"]?.Invoke();
        }

        private void OnFinalize()
        {
            _cachedMethods["OnFinalize"]?.Invoke();
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
