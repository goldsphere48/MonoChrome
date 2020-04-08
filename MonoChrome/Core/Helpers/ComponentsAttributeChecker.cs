using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers
{
    static class ComponentsAttributeChecker
    {
        public static bool Verify(Component component, GameObject gameObject)
        {
            var componentTypeInfo = component.GetType();
            var componentAttributes = componentTypeInfo.GetCustomAttributes(false);
            var attributeVisitor = new ComponentAttributeVisitor(component, gameObject);
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
