using System;

namespace MonoChrome.Core.Exceptions
{
    public class SceneNotInitializedException : Exception
    {
        public SceneNotInitializedException(Type sceneType) : base($"Scene with id: {sceneType.Name} not initialized")
        {
        }
    }
}