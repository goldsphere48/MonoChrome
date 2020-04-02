using Match_3.Source.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Scenes
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
        private Dictionary<SceneId, SceneController> _scenes = new Dictionary<SceneId, SceneController>();
        private static SceneManager _instance;
        

        protected SceneManager()
        {

        }

        public void Register(Scene scene)
        {
            Debug.Assert(scene != null);
            if (Contains(scene.Id))
            {
                throw new SceneAlreadyExistException(scene.Id);
            }
            _scenes.Add(scene.Id, new SceneController(scene, GraphicsDevice));
        }
        
        public void LoadScene(SceneId sceneId) 
        {
            Debug.Assert(sceneId != null);
            if (!Contains(sceneId))
            {
                throw new SceneNotFoundException(sceneId);
            }
            var scene = _scenes[sceneId];
            if (!scene.IsInitialized)
            {
                scene.Initialize();
            }
        }

        public void UnloadScene(SceneId sceneId) 
        {
            Debug.Assert(sceneId != null);
            if (!Contains(sceneId))
            {
                throw new SceneNotFoundException(sceneId);
            }
            var scene = _scenes[sceneId];
            if (scene.IsInitialized)
            {
                scene.Dispose();
            }
        }

        public void UnloadAll()
        {
            foreach (var value in _scenes.Values)
            {
                value.Disable();
                value.Dispose();
            }
        }

        public void SetActiveScene(SceneId sceneId)
        {
            Debug.Assert(sceneId != null);
            if (!Contains(sceneId))
            {
                throw new SceneNotFoundException(sceneId);
            }
            var scene = _scenes[sceneId];
            DisableScene(_currentScreen);
            _currentScreen = scene;
            InitializeOrEnable(_currentScreen);
        }

        public void DrawActiveScreen(GameTime gameTime)
        {
            _currentScreen.Draw(gameTime);
        }

        public void UpdateActiveScreen(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
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

        private bool Contains(SceneId id)
        {
            return _scenes.Keys.Contains(id);
        }

        private void DisableScene(SceneController _scene)
        {
            _currentScreen.Disable();
        }
    }
}
