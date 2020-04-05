using MonoChrome.Core.GameObjectSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.SceneSystem
{
    public abstract class Scene : Playable
    {
        private Group _root = new Group();

        public SpriteBatch SpriteBatch { get; set; }

        public void Add(GameObject gameObject)
        {
            _root.Add(gameObject);
        }
        public void Remove(GameObject gameObject)
        {
            _root.Remove(gameObject);
        }

        #region IPlayable
        public override void Awake()
        {

        }
        public override void Start()
        {
            throw new System.NotImplementedException();
        }
        public override void Update()
        {
            _root.Update();
        }
        public override void OnDisable()
        {
            _root.OnDisable();
        }
        public override void OnEnable()
        {
            _root.OnEnable();
        }
        public override void OnDestroy()
        {
            _root.OnDestroy();
        }
        public override void OnFinalize()
        {
            _root.OnFinalize();
        }
        #endregion
    }
}
