using MonoChrome.Core.Attributes;
using MonoChrome.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    public class ComponentAttributeVisitor
    {
        public Type CurrentComponent { private get; set; }
        public IEnumerable<Type> ComponentTypes { private get; set; }

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
