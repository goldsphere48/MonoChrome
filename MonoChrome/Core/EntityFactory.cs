using MonoChrome.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core
{
    internal class EntityFactory
    {
        private EntityStore _entites;
        public EntityFactory(EntityStore entities)
        {
            _entites = entities;
        }

        public GameObject CreateEmpty()
        {
            return CreateEmpty(GameObject.DefaultName;);
        }

        public GameObject CreateEmpty(string name)
        {
            string uniqueName = name;
            if (_entites.ContainsKey(name))
            {
                uniqueName = GetUniqueName(name);
            }
            var gameObject = Construct(uniqueName);
            gameObject.AddComponent<Transform>();
            return gameObject;
        }

        private string GetUniqueName(string startWith)
        {
            var number = _entites.FindAll(gameObject => gameObject.Name.StartsWith(startWith)).Count + 1;
            return $"{startWith}({number})";
        }

        private static GameObject Construct(string name)
        {
            Type t = typeof(GameObject);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(string) }, null);

            return ci.Invoke(new object[] { name }) as GameObject;
        }
    }
}
