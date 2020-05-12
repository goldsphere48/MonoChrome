namespace MonoChrome.Core.Components.CollisionDetection
{
    public class Collision
    {
        public GameObject GameObject { get; }
        public Collision(GameObject obj)
        {
            GameObject = obj;
        }
    }
}