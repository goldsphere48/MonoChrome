using MonoChrome.Core.EntityManager;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    public class FieldAttributeVisitor
    {
        public Component CurrentComponent { private get; set; }
        public FieldInfo CurrentField { private get; set; }
        public IEnumerable<Component> Components { private get; set; }
        public ICollection<AttributeError> CheckResults { get; set; } = new List<AttributeError>();
        internal void VisitInsertGameObjectAttribute(string name, bool required)
        {
            if (CurrentField.FieldType == typeof(GameObject))
            {
                var gameObject = Entity.Find(name);
                if (gameObject == null && required)
                {
                    CheckResults.Add(new AttributeError { Message = $"Can't find game object with name {name}" });
                }
                else
                {
                    CurrentField.SetValue(CurrentComponent, gameObject, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
                }
            }
            else
            {
                CheckResults.Add(new AttributeError { Message = $"Can't insert game object with name {name} to not game object field" });
            }
        }
        public void VisitInsertComponentAttribute(string from, bool inherit, bool required)
        {
            if (!string.IsNullOrEmpty(from))
            {
                var gameObject = Entity.Find(from);
                if (gameObject != null)
                {
                    Components = gameObject.GetComponents();
                }
                else
                {
                    return;
                }
            }
            foreach (var component in Components)
            {
                if (component.GetType() == CurrentField.FieldType || (inherit && CurrentField.FieldType.IsAssignableFrom(component.GetType())))
                {
                    CurrentField.SetValue(CurrentComponent, component, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
                    return;
                }
            }
            if (required == true)
            {
                CheckResults.Add(new AttributeError { Message = $"Unfound required component {CurrentField.FieldType.Name}" });
            }
        }
    }
}
