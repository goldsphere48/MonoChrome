using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    internal class EntityStore
    {
        private IDictionary<string, GameObject> _gameObjects = new Dictionary<string, GameObject>();
        public void Add(GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException("Can't add null to Entity Store");
            }
            _gameObjects.Add(gameObject.Name, gameObject);
        }

        public GameObject Get(string name)
        {
            if (name == null || !_gameObjects.ContainsKey(name))
            {
                return null;
            }
            return _gameObjects[name];
        }

        public bool Remove(GameObject gameObject)
        {
            return _gameObjects.Remove(gameObject.Name);
        }

        public List<GameObject> FindAll(Predicate<GameObject> predicate)
        {
            return _gameObjects.Values.ToList().FindAll(predicate);
        }

        public GameObject Find(Predicate<GameObject> predicate)
        {
            return _gameObjects.Values.ToList().Find(predicate);
        }

        public bool ContainsKey(string name)
        {
            return _gameObjects.ContainsKey(name);
        }
    }
}
