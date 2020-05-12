using Microsoft.Xna.Framework;
using System;

namespace MonoChrome.SceneSystem
{
    internal interface ISceneManager
    {
        void Clear();
        void ClearAllExceptCurrent();
        void Draw();
        bool IsLoaded<T>() where T : IScene;
        bool IsLoaded(Type sceneType);
        void LoadScene<T>() where T : IScene;
        void LoadScene(Type sceneType);
        void SetActiveScene<T>() where T : IScene;
        void SetActiveScene(Type sceneType);
        void UnloadScene<T>() where T : IScene;
        void UnloadScene(Type sceneType);
        void Update(GameTime gameTime);
    }
}