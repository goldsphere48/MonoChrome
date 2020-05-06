using MonoChrome.Core.EntityManager;
using MonoChrome.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    public class FieldAttributeVisitor
    {
        public Component CurrentComponent { private get; set; }
        public FieldInfo CurrentField { private get; set; }
        public IEnumerable<Component> Components { private get; set; }

        public void VisitInsertComponentAttribute(string from)
        {
            if (!string.IsNullOrEmpty(from))
            {
                var gameObject = Entity.Find(from);
                if (gameObject != null)
                {
                    Components = gameObject.GetComponents();
                } else
                {
                    return;
                }
            }
            foreach (var component in Components)
            {
                if (component.GetType() == CurrentField.FieldType)
                {
                    CurrentField.SetValue(CurrentComponent, component, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
                    return;
                }
            }
            throw new UnfoundRequiredComponentsException($"Unfound required component {CurrentField.FieldType.Name}");
        }
    }
}
