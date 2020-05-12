﻿using MonoChrome.Core.EntityManager;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    public class FieldAttributeVisitor
    {
        public ICollection<AttributeError> CheckResults { get; set; } = new List<AttributeError>();
        public IEnumerable<Component> Components { private get; set; }
        public Component CurrentComponent { private get; set; }
        public FieldInfo CurrentField { private get; set; }
        private IEnumerable<Component> _components;

        public void VisitInsertComponentAttribute(string from, bool inherit, bool required)
        {
            if (!string.IsNullOrEmpty(from))
            {
                var gameObject = Entity.Find(from);
                if (gameObject != null)
                {
                    _components = Components;
                    Components = gameObject.GetComponents();
                }
                else
                {
                    return;
                }
            } else if (_components != null)
            {
                Components = _components;
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
    }
}