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
    public class SceneManager : ISceneManager
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
        

        protected SceneManager()
        {

        }

        #region ISceneMagar interface
        public T CreateEmpty<T>() where T : IScene
        {
            return CreateEmpty(typeof(T)) as T;
        }

        public IScene CreateEmpty(Type type)
        {
            if (IsScene(type))
            {
                return Activator.CreateInstance(type) as IScene;
            }
            return null;
        }

        public void LoadScene(Type type)
        {
            if (IsScene(type) && !Contains(type))
            {
                throw new SceneNotFoundException(type);
            }
            var scene = GetSceneController(type);
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
            if (IsScene(type) && !Contains(type))
            {
                throw new SceneNotFoundException(type);
            }
            var scene = GetSceneController(type);
            if (scene.Initialized)
            {
                ((IDisposable)scene).Dispose();
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
                UnloadScene(value.GetType());
            }
            _scenes.Clear();
        }

        public void Flush()
        {
            foreach (var value in _scenes)
            {
                if (_currentScreen != value)
                {
                    UnloadScene(value.GetType());
                    _scenes.Remove(value);
                }
            }
        }

        public void SetActiveScene(Type type)
        {
            if (IsScene(type) && !Contains(type))
            {
                throw new SceneNotFoundException(type);
            }
            var scene = GetSceneController(type);
            if (!scene.Initialized)
            {
                throw new SceneNotInitializedException(type);
            }
            DisableScene(_currentScreen);
            _currentScreen = scene;
            InitializeOrEnable(_currentScreen);
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
        #endregion

        public void DrawActiveScreen()
        {
            _currentScreen?.Draw();
        }

        public void UpdateActiveScreen()
        {
            _currentScreen?.Update();
        }

        internal SceneController GetSceneController(Type type)
        {
            return _scenes.Find(sceneController => sceneController.SceneType == type);
        }

        internal SceneController GetSceneController<T>() where T : IScene
        {
            return GetSceneController(typeof(T));
        }

        private bool IsScene(Type type)
        {
            return type.IsAssignableFrom(typeof(IScene));
        }

        private bool Contains(Type sceneType)
        {
            return _scenes.Any(sceneController => sceneType == sceneController.SceneType);
        }

        private void InitializeOrEnable(SceneController scene)
        {
            Debug.Assert(scene != null);
            if (!scene.Initialized)
            {
                scene.Setup();
            }
            else
            {
                scene.OnEnable();
            }
        }

        private void DisableScene(SceneController _scene)
        {
            _scene?.Dispose();
        }
    }
}
