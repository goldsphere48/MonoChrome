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

        internal void Attach(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
