using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.GameObjectSystem.Components
{
    public class Component : Playable
    {
        public GameObject GameObject { get; private set; }

        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }

        public Component GetComponent(Type componentType)
        {
            return GameObject.GetComponent(componentType);
        }

        public override void Awake()
        {

        }

        public override void OnDestroy()
        {

        }

        public override void OnDisable()
        {

        }

        public override void OnEnable()
        {

        }

        public override void OnFinalize()
        {
        }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        /// <summary>
        /// Attach gameObject to Component with reflection
        /// </summary>
        /// <param name="gameObject"></param>
        internal void Attach(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
