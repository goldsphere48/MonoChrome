using Match_3.Source.Core.GameObjectSystem.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Core.GameObjectSystem
{
    class Group : GameObject
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
