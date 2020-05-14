using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.SceneSystem
{
    public sealed class SceneManager : ISceneManager
    {
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
        public ContentManager Content { get; set; }
        public Game Game { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
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
        public Scene CurrentScene => _currentScreen.GetScene();
        public void Draw()
        {
            _currentScreen?.Draw();
        }
        public bool IsLoaded(Type type)
        {
            return _scenes.Find(scene => scene.SceneType == type).Initialized;
        }
        public bool IsLoaded<T>() where T : IScene
        {
            return IsLoaded(typeof(T));
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
                scene = new SceneController(type, GraphicsDevice, Content, Game);
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
        public void Update(GameTime gameTime)
        {
            Time.GameTime = gameTime;
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
        private SceneManager()
        {
        }
        private static SceneManager _instance;
        private SceneController _currentScreen;
        private List<SceneController> _scenes = new List<SceneController>();
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