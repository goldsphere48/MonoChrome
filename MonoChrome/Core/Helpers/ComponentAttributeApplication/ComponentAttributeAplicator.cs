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
        private static FieldAttributeVisitor _fieldAttributeVisitor = new FieldAttributeVisitor();
        
        public static IEnumerable<AttributeError> Apply(Component component)
        {
            _fieldAttributeVisitor.CheckResults.Clear();
            _fieldAttributeVisitor.Components = component.GameObject.GetComponents();
            ProceedComponentFields(component);
            return _fieldAttributeVisitor.CheckResults;
        }

        private static void ProceedComponentFields(Component component)
        {
            _fieldAttributeVisitor.CurrentComponent = component;
            var fields = GetAllFields(component.GetType());
            foreach (var field in fields)
            {
                ProceedField(field);
            }
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            if (type == null && !typeof(Component).IsAssignableFrom(type))
            {
                return Enumerable.Empty<FieldInfo>();
            }
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Concat(GetAllFields(type.BaseType));
        }

        private static void ProceedField(FieldInfo field)
        {
            _fieldAttributeVisitor.CurrentField = field;
            var attributes = field.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                if (attribute is IComponentApplicatorAcceptable accepltableAttribute)
                {
                    ProceedFieldAttribute(accepltableAttribute);
                }
            }
        }

        private static void ProceedFieldAttribute(IComponentApplicatorAcceptable attribute)
        {
            attribute.AcceptFieldVisitor(_fieldAttributeVisitor);
        }
    }
}
