using Microsoft.Xna.Framework;
using MonoChrome.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.LayerManager
{
    class Layer : ICollection<GameObject>
    {
        public string Name { get; }
        public int ZIndex { get; set; }
        public int Count => _gameObjects.Count;
        public bool IsReadOnly => false;
        public bool CollisionDetectionEnable { get; set; } = true;
        public bool HandleClickEnable { get; set; } = true;

        private ICollection<GameObject> _gameObjects = new HashSet<GameObject>();

        public Layer(string name, int zIndex)
        {
            Name = name;
            ZIndex = zIndex;
        }

        public void Add(GameObject item)
        {
            item.LayerName = Name;
            _gameObjects.Add(item);
        }

        public void Clear()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.LayerName = null;
            }
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

        public bool Remove(GameObject item)
        {
            item.LayerName = null;
            return _gameObjects.Remove(item);
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
