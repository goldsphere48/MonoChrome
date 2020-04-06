using MonoChrome.Core.GameObjectSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoChrome.Core.SceneSystem
{
    public abstract class Scene : Playable
    {
        private GameObject _root = new GameObject();

        public SpriteBatch SpriteBatch { get; set; }
        public GameObject Root => _root;

        public void Add(GameObject gameObject)
        {
            _root.Transform.Parent = gameObject.Transform;
        }
        public void Remove(GameObject gameObject)
        {
            _root.Transform.Parent = null;
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
