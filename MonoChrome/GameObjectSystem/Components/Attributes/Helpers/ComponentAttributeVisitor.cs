using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes.Helpers
{
    public class ComponentAttributeVisitor
    {
        private Component _component;
        private GameObject _gameObject;

        public ComponentAttributeVisitor(Component component, GameObject gameObject)
        {
            _component = component;
            _gameObject = gameObject;
        }

        public void VisitComponentUsageAttribute(ComponentUsageAttribute attribute)
        {
            if (!attribute.AllowMultipleComponentUsage)
            {
                if (_gameObject.GetComponent(_component.GetType()) != null)
                {
                    throw new InvalidComponentDuplicateException(_component.GetType());
                }
            }
        }

        public void VisitRequireComponentAttribute(RequireComponentAttribute attribute)
        {
            var requiredComponent = _gameObject.GetComponent(attribute.RequiredComponent);
            if (requiredComponent == null)
            {
                throw new UnfoundRequiredComponentsException(attribute.RequiredComponent);
            }
        }

        public void VisitCreatedForAttribute(CreatedForAttribute attribute)
        {
            bool inherit = attribute.Inherit;
            bool isSameOrSubclass = attribute.TargetType.IsAssignableFrom(_gameObject.GetType());
            bool isSame = attribute.TargetType == _gameObject.GetType();
            bool notCorrect = (inherit && !isSameOrSubclass) || (!inherit && !isSame);
            if (notCorrect)
            {
                throw new InvalidComponentTargetException(_component.GetType(), _gameObject.GetType());
            }
        }
    }
}
