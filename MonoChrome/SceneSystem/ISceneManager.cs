using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem
{
    interface ISceneManager
    {
        T Create<T>() where T : class, IScene;
        IScene Create(Type type);
        void LoadScene<T>() where T : IScene;
        void LoadScene(Type sceneType);
        void UnloadScene<T>() where T : IScene;
        void UnloadScene(Type sceneType);
        void SetActiveScene<T>() where T : IScene;
        void SetActiveScene(Type sceneType);
        bool IsLoaded<T>() where T : IScene;
        bool IsLoaded(Type sceneType);
        void ClearAllExceptCurrent();
        void DrawActiveScene();
        void UpdateActiveScene();
        void Clear();
    }
}
