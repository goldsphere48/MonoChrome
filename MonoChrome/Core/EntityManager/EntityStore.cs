using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class EntityStore : IEntityCollection<GameObject>
    {
        private HashSet<GameObject> _gameObjects = new HashSet<GameObject>();
        public int Count => _gameObjects.Count;

        public bool IsReadOnly => false;

        public void Add(GameObject item)
        {
            _gameObjects.Add(item);
        }

        public void Clear()
        {
            _gameObjects.Clear();
        }

        public bool Contains(GameObject item)
        {
            return _gameObjects.Contains(item);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _gameObjects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        public bool Remove(GameObject item)
        {
            return _gameObjects.Remove(item);
        }

        public void Synchronize()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
