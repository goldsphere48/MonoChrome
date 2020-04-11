using MonoChrome.Core.Attributes;
using MonoChrome.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers
{
    public class ComponentAttributeVisitor
    {
        private Type _componentType;
        private Type[] _otherComponentTypes;

        public ComponentAttributeVisitor(Type componentType, Type[] otherComponentTypes)
        {
            _componentType = componentType;
            _otherComponentTypes = otherComponentTypes;
        }

        public void VisitComponentUsageAttribute(ComponentUsageAttribute attribute)
        {
            if (!attribute.AllowMultipleComponentUsage)
            {
                var sameComponents = _otherComponentTypes.ToList().FindAll(component => component == _componentType);
                if (sameComponents.Count > 1)
                {
                    throw new InvalidComponentDuplicateException($"Found invalid duplicate of component {_componentType.Name}");
                }
            }
        }

        public void VisitRequireComponentAttribute(RequireComponentAttribute attribute)
        {
            var requiredComponent = _otherComponentTypes.ToList().Find(component => component == attribute.RequiredComponent);
            if (requiredComponent == null)
            {
                throw new UnfoundRequiredComponentsException($"Unfound required component {attribute.RequiredComponent.Name}");
            }
        }
    }
}
