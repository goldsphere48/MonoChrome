using MonoChrome.Core.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem
{
    public sealed class SceneManager : ISceneManager
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }
                return _instance;
            }
        }

        private static SceneManager _instance;
        private SceneController _currentScreen;
        private List<SceneController> _scenes = new List<SceneController>();

        private SceneManager()
        {

        }

        #region ISceneManager interface
        public T Create<T>() where T : class, IScene
        {
            return Create(typeof(T)) as T;
        }

        public IScene Create(Type type)
        {
            if (IsScene(type))
            {
                return Activator.CreateInstance(type) as IScene;
            }
            return null;
        }

        public void LoadScene(Type type)
        {
            if (!IsScene(type))
            {
                throw new ArgumentException($"{type.Name} is not subclass of IScene");
            }
            var scene = GetSceneController(type);
            if (scene == null)
            {
                scene = new SceneController(Create(type), GraphicsDevice);
                _scenes.Add(scene);
            }
            if (!scene.Initialized)
            {
                scene.Setup();
            }
        }

        public void LoadScene<T>() where T : IScene
        {
            LoadScene(typeof(T));
        }

        public void UnloadScene(Type type)
        {
            if (!IsScene(type) && !Contains(type))
            {
                throw new ArgumentException($"{type.Name} is not subclass of IScene or this scene doesn't exist");
            }
            var scene = GetSceneController(type);
            if (scene.Initialized)
            {
                scene.Dispose();
            }
            _scenes.Remove(scene);
        }

        public void UnloadScene<T>() where T : IScene
        {
            UnloadScene(typeof(T));
        }

        public void Clear()
        {
            foreach (var value in _scenes)
            {
                var scene = GetSceneController(value.SceneType);
                if (scene.Initialized)
                {
                    scene.Dispose();
                }
            }
            _scenes.Clear();
        }

        public void ClearAllExceptCurrent()
        {
            var scenesToRemove = new List<Type>();
            foreach (var value in _scenes)
            {
                if (_currentScreen != value)
                {
                    var scene = GetSceneController(value.SceneType);
                    scenesToRemove.Add(value.SceneType);
                    if (scene.Initialized)
                    {
                        scene.Dispose();
                    }
                }
            }
            for (int i = 0; i < scenesToRemove.Count; ++i)
            {
                var scene = GetSceneController(scenesToRemove[i]);
                _scenes.Remove(scene);
            }
        }

        public void SetActiveScene(Type type)
        {
            var scene = GetSceneController(type);
            if (scene == null)
            {
                LoadScene(type);
                scene = GetSceneController(type);
            }
            _currentScreen?.OnDisable();
            _currentScreen = scene;
            if (!scene.Initialized)
            {
                scene.Setup();
            }
            scene.OnEnable();
        }

        public void SetActiveScene<T>() where T : IScene
        {
            SetActiveScene(typeof(T));
        }

        public bool IsLoaded(Type type)
        {
            return _scenes.Find(scene => scene.SceneType == type).Initialized;
        }

        public bool IsLoaded<T>() where T : IScene
        {
            return IsLoaded(typeof(T));
        }

        public void DrawActiveScene()
        {
            _currentScreen?.Draw();
        }

        public void UpdateActiveScene()
        {
            _currentScreen?.Update();
        }
        #endregion

        internal SceneController GetSceneController(Type type)
        {
            return _scenes.Find(sceneController => sceneController.SceneType == type);
        }

        internal SceneController GetSceneController<T>() where T : IScene
        {
            return GetSceneController(typeof(T));
        }

        private static bool IsScene(Type type)
        {
            return typeof(IScene).IsAssignableFrom(type);
        }

        private bool Contains(Type sceneType)
        {
            return _scenes.Any(sceneController => sceneType == sceneController.SceneType);
        }
    }
}
