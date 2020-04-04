using Match_3.Source.Core.GameObjectSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3.Source.Core.SceneSystem
{
    abstract class Scene : Playable
    {
        private Group _root = new Group();
        private bool _enabled = true;

        public SpriteBatch SpriteBatch { get; set; }
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }

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
