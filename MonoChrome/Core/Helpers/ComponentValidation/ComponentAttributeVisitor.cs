using MonoChrome.Core.Attributes;
using MonoChrome.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentValidation
{
    public class ComponentAttributeVisitor
    {
        public Type CurrentComponent { private get; set; }
        public IEnumerable<Type> ComponentTypes { private get; set; }

        public void VisitComponentUsageAttribute(ComponentUsageAttribute attribute)
        {
            if (!attribute.AllowMultipleComponentUsage)
            {
                var sameComponents = ComponentTypes.ToList().FindAll(component => component == CurrentComponent);
                if (sameComponents.Count > 1)
                {
                    throw new InvalidComponentDuplicateException($"Found invalid duplicate of component {CurrentComponent.Name}");
                }
            }
        }

        public void VisitRequireComponentAttribute(RequireComponentAttribute attribute)
        {
            var requiredComponent = ComponentTypes.ToList().Find(component => component == attribute.RequiredComponent);
            if (requiredComponent == null)
            {
                throw new UnfoundRequiredComponentsException($"Unfound required component {attribute.RequiredComponent.Name}");
            }
        }
    }
}
