using MonoChrome.Core.GameObjectSystem.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.GameObjectSystem
{
    public class Group : GameObject
    {
        public void Add(GameObject gameObject)
        {
            var parentTransform = GetComponent<Transform>();
            var childTransform = gameObject.GetComponent<Transform>();
            childTransform.Parent = parentTransform;
        }

        public void Remove(GameObject gameObject)
        {
            var parentTransform = GetComponent<Transform>();
            var childTransform = gameObject.GetComponent<Transform>();
            parentTransform.Childrens.Remove(childTransform);
        }
    }
}
