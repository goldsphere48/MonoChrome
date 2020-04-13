using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class EntityEventArgs : EventArgs
    {
        public GameObject GameObject { get; private set; }
        public EntityEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
