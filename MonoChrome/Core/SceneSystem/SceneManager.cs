using Match_3.Source.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Core.SceneSystem
{
    class SceneManager
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

        private SceneController _currentScreen;
        private List<SceneController> _scenes = new List<SceneController>();
        private static SceneManager _instance;
        

        protected SceneManager()
        {

        }

        private bool Contains(Type sceneType)
        {
            return _scenes.Any(sceneController => sceneType == sceneController.Scene.GetType());
        }

        public SceneController GetSceneController<T>() where T : Scene
        {
            return _scenes.Find(sceneController => sceneController.Scene.GetType() == typeof(T));
        }

        public void Register<T>() where T : Scene
        {
            var scene = Activator.CreateInstance(typeof(T)) as Scene;
            var sceneType = typeof(T);
            if (Contains(sceneType))
            {
                throw new SceneAlreadyExistException(sceneType);
            }
            _scenes.Add(new SceneController(scene, GraphicsDevice));
        }
        
        public void LoadScene<T>() where T : Scene
        {
            var sceneType = typeof(T);
            if (!Contains(sceneType))
            {
                throw new SceneNotFoundException(sceneType);
            }
            var scene = GetSceneController<T>();
            if (!scene.IsInitialized)
            {
                scene.Initialize();
            }
        }

        public void UnloadScene<T>() where T : Scene
        {
            var sceneType = typeof(T);
            if (!Contains(sceneType))
            {
                throw new SceneNotFoundException(sceneType);
            }
            var scene = GetSceneController<T>();
            if (scene.IsInitialized)
            {
                ((IDisposable)scene).Dispose();
            }
        }

        public void UnloadAll()
        {
            foreach (var value in _scenes)
            {
                value.Disable();
                ((IDisposable)value).Dispose();
            }
        }

        public void SetActiveScene<T>() where T : Scene
        {
            var sceneType = typeof(T);
            if (!Contains(sceneType))
            {
                throw new SceneNotFoundException(sceneType);
            }
            var scene = GetSceneController<T>();
            if (!scene.IsInitialized)
            {
                throw new SceneNotInitializedException(sceneType);
            }
            DisableScene(_currentScreen);
            _currentScreen = scene;
            InitializeOrEnable(_currentScreen);
        }

        public void DrawActiveScreen()
        {
            _currentScreen?.Draw();
        }

        public void UpdateActiveScreen()
        {
            _currentScreen?.Update();
        }

        private void InitializeOrEnable(SceneController scene)
        {
            Debug.Assert(scene != null);
            if (!scene.IsInitialized)
            {
                scene.Initialize();
            }
            else
            {
                scene.Enable();
            }
        }

        private void DisableScene(SceneController _scene)
        {
            _currentScreen?.Disable();
        }
    }
}
