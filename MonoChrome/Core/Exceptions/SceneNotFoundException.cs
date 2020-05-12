using System;

namespace MonoChrome.Core.Exceptions
{
    public class SceneNotFoundException : Exception
    {
        public SceneNotFoundException(Type sceneType) : base($"Scene with id: {sceneType.Name} not founded")
        {
        }
    }
}
