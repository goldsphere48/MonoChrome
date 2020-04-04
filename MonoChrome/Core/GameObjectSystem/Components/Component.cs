using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Core.GameObjectSystem.Components
{
    class Component : Playable
    {
        public GameObject GameObject { get; private set; }

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
        private void Attach(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
