using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    public class Component : Playable
    {
        public GameObject GameObject { get; private set; }

        internal void Attach(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        internal void Dettach()
        {
            GameObject = null;
        }

        public static Component Create(Type componentType)
        {
            Component result = null;
            try
            {
                result = Activator.CreateInstance(componentType) as Component;
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException(string.Format(
                    "The component type '{0}' does not provide a parameter-less constructor.", componentType.ToString()));
            }
            return result;
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
    }
}
