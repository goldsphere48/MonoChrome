using System;

namespace MonoChrome.Core.Exceptions
{
    public class SceneAlreadyExistException : Exception
    {
        public SceneAlreadyExistException(Type sceneType) : base($"Scene with id: {sceneType.Name} is already exist")
        {
        }
    }
}
