using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    internal class DependencieFieldInfo
    {
        public FieldInfo Field { get; set; }
        public string From { get; set; }
        public bool Inherit { get; set; }
        public Component Component { get; set; }
        public void Inject<T>(T value)
        {
            Field.SetValue(Component, value, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
        }
    }

    public class FieldAttributeVisitor
    {
        internal List<DependencieFieldInfo> DependencieComponents { get; } = new List<DependencieFieldInfo>();
        internal List<DependencieFieldInfo> DependencieObjects { get; } = new List<DependencieFieldInfo>();
        internal Component CurrentComponent { private get; set; }
        internal FieldInfo CurrentField { private get; set; }

        internal void VisitInsertComponentAttribute(string from, bool inherit, bool required)
        {
            DependencieComponents.Add(
                new DependencieFieldInfo
                {
                    Field = CurrentField,
                    Component = CurrentComponent,
                    From = from,
                    Inherit = inherit
                });
        }

        internal void VisitInsertGameObjectAttribute(string name, bool required)
        {
            if (CurrentField.FieldType == typeof(GameObject))
            {
                DependencieObjects.Add(new DependencieFieldInfo
                {
                    Field = CurrentField,
                    Component = CurrentComponent,
                    From = name
                });
            }
            else
            {
                throw new InvalidOperationException($"Can't insert game object with name {name} to not game object field");
            }
        }
    }
}