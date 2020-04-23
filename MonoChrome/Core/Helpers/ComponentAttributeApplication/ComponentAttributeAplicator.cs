using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    static class ComponentAttributeAplicator
    {
        private static ComponentAttributeVisitor _componentAttributeVisitor = new ComponentAttributeVisitor();
        private static FieldAttributeVisitor _fieldAttributeVisitor = new FieldAttributeVisitor();
        public static bool Valid(IEnumerable<Type> componentTypes)
        {
            _componentAttributeVisitor.ComponentTypes = componentTypes;
            foreach (var componentType in componentTypes) 
            {
                _componentAttributeVisitor.CurrentComponent = componentType;
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

        public static bool Apply(IEnumerable<Component> components)
        {
            _fieldAttributeVisitor.Components = components;
            foreach (var component in components)
            {
                ProceedComponentFields(component);
            }
            return true;
        }

        public static bool Apply(Component component, IEnumerable<Component> components)
        {
            _fieldAttributeVisitor.Components = components;
            ProceedComponentFields(component);
            return true;
        }

        private static void ProceedComponentFields(Component component)
        {
            _fieldAttributeVisitor.CurrentComponent = component;
            var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                ProceedField(field);
            }
        }

        private static void ProceedField(FieldInfo field)
        {
            _fieldAttributeVisitor.CurrentField = field;
            var attributes = field.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                var accepltableAttribute = attribute as IComponentApplicatorAcceptable;
                if (accepltableAttribute != null)
                {
                    ProceedFieldAttribute(accepltableAttribute);
                }
            }
        }

        private static void ProceedFieldAttribute(IComponentApplicatorAcceptable attribute)
        {
            if (attribute.Visited)
            {
                return;
            }
            attribute.AcceptFieldVisitor(_fieldAttributeVisitor);
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
            if (componentAttribute is IComponentValidatorAcceptable)
            {
                var acceptatbleAttribute = componentAttribute as IComponentValidatorAcceptable;
                if (acceptatbleAttribute.Visited)
                {
                    return;
                }
                acceptatbleAttribute.AcceptComponentVisitor(_componentAttributeVisitor);
            }
        }
    }
}
