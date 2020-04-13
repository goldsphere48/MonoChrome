using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentValidation
{
    static class ComponentValidator
    {
        private static ComponentAttributeVisitor _attributeVisitor = new ComponentAttributeVisitor();
        public static bool Valid(IEnumerable<Type> componentTypes)
        {
            _attributeVisitor.ComponentTypes = componentTypes;
            foreach (var componentType in componentTypes) {
                _attributeVisitor.CurrentComponent = componentType;
                ProceedComponent(componentType, componentTypes);
            }
            return true;
        }

        public static bool Valid(Type componentType, IEnumerable<Type> componentTypes)
        {
            ProceedComponent(componentType, componentTypes);
            return true;
        }

        public static bool Valid(Component component, IEnumerable<Component> components)
        {
             var componentTypes = components.Select(item => component.GetType());
            ProceedComponent(component.GetType(), componentTypes);
            return true;
        }

        public static bool Valid(IEnumerable<Component> components)
        {
            var componentTypes = components.Select(component => component.GetType());
            return Valid(componentTypes);
        }

        private static void ProceedComponent(Type componentType, IEnumerable<Type> otherComponentTypes)
        {
            var componentAttributes = componentType.GetCustomAttributes(false);
            foreach (var componentAttribute in componentAttributes)
            {
                ProceedAttribute(componentAttribute);
            }
        }

        private static void ProceedAttribute(object componentAttribute)
        {
            if (componentAttribute is IComponentVisitorAcceptable)
            {
                var acceplatbleAttribute = componentAttribute as IComponentVisitorAcceptable;
                acceplatbleAttribute.AcceptVisitor(_attributeVisitor);
            }
        }
    }
}
