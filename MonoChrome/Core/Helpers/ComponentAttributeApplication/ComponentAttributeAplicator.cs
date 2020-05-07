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
            attribute.AcceptFieldVisitor(_fieldAttributeVisitor);
        }
    }
}
