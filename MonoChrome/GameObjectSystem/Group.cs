using MonoChrome.Core.GameObjectSystem.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.GameObjectSystem
{
    public class Group : GameObject, IEnumerator<GameObject>, IEnumerable<GameObject>
    {
        private int _currentPosition = -1;
        object IEnumerator.Current => Transform.Childrens[_currentPosition].GameObject;
        public GameObject Current => Transform.Childrens[_currentPosition].GameObject;
        public int Count => Transform.Childrens.Count;

        public void Add(GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException();
            }
            var childTransform = gameObject.GetComponent<Transform>();
            childTransform.Parent = Transform;
        }

        public void Dispose()
        {
            Reset();
        }

        public bool MoveNext()
        {
            if (_currentPosition < Transform.Childrens.Count - 1)
            {
                _currentPosition++;
                return true;
            }
            return false;
        }

        public bool Remove(GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException();
            }
            var childTransform = gameObject.Transform;
            if (Transform.Childrens.Contains(childTransform))
            {
                childTransform.Parent = null;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _currentPosition = -1;
        }

        IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
