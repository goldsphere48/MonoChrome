namespace MonoChrome.SceneSystem
{
    public interface IScene
    {
        void OnDisable();
        void OnEnable();
        void Setup();
    }
}