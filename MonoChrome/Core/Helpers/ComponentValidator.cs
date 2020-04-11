using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers
{
    static class ComponentValidator
    {
        public static bool Verify(Type componentType, Type[] otherComponentTypes)
        {
            var componentAttributes = componentType.GetCustomAttributes(false);
            var attributeVisitor = new ComponentAttributeVisitor(componentType, otherComponentTypes);
            foreach (var componentAttribute in componentAttributes)
            {
                if (componentAttribute is IComponentVisitorAcceptable)
                {
                    var acceplatbleAttribute = componentAttribute as IComponentVisitorAcceptable;
                    acceplatbleAttribute.AcceptVisitor(attributeVisitor);
                }
            }
            return true;
        }
    }
}
