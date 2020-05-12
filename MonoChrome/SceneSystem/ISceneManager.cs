using Microsoft.Xna.Framework;
using System;

namespace MonoChrome.SceneSystem
{
    interface ISceneManager
    {
        void LoadScene<T>() where T : IScene;
        void LoadScene(Type sceneType);
        void UnloadScene<T>() where T : IScene;
        void UnloadScene(Type sceneType);
        void SetActiveScene<T>() where T : IScene;
        void SetActiveScene(Type sceneType);
        bool IsLoaded<T>() where T : IScene;
        bool IsLoaded(Type sceneType);
        void ClearAllExceptCurrent();
        void Draw();
        void Update(GameTime gameTime);
        void Clear();
    }
}
