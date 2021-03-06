﻿using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoChrome.Core.Helpers.FieldInjection
{
    public class FieldAttributeVisitor
    {
        internal Component CurrentComponent { private get; set; }
        internal FieldInfo CurrentField { private get; set; }
        internal List<DependencyFieldInfo> DependencyComponents { get; } = new List<DependencyFieldInfo>();
        internal List<DependencyFieldInfo> DependencyObjects { get; } = new List<DependencyFieldInfo>();

        internal void VisitInsertComponentAttribute(string from, bool inherit, bool required)
        {
            var component = FindComponent(from, inherit, CurrentField.FieldType);
            var isInOrder = true;
            if (component != null)
            {
                isInOrder = false;
                CurrentField.SetValue(CurrentComponent, component, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
            }
            DependencyComponents.Add(
               new DependencyFieldInfo
               {
                   Field = CurrentField,
                   Component = CurrentComponent,
                   From = from,
                   Inherit = inherit,
                   IsInOrder = isInOrder,
                   Required = required
               });
        }

        internal void VisitInsertGameObjectAttribute(string name, bool required)
        {
            if (CurrentField.FieldType == typeof(GameObject))
            {
                var gameObject = Entity.Find(name);
                var isInOrder = true;
                if (gameObject != null)
                {
                    isInOrder = false;
                    CurrentField.SetValue(CurrentComponent, gameObject, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
                }
                DependencyObjects.Add(new DependencyFieldInfo
                {
                    Field = CurrentField,
                    Component = CurrentComponent,
                    From = name,
                    IsInOrder = isInOrder,
                    Required = required
                });
            }
            else
            {
                throw new InvalidOperationException($"Can't insert game object with name {name} to not game object field");
            }
        }

        private Component FindComponent(string from, bool inherit, Type componentType)
        {
            if (string.IsNullOrEmpty(from))
            {
                return CurrentComponent.GetComponent(componentType, inherit);
            }
            else
            {
                return Entity.Find(from)?.GetComponent(componentType, inherit);
            }
        }
    }

    internal class DependencyFieldInfo
    {
        public Component Component { get; set; }
        public FieldInfo Field { get; set; }
        public string From { get; set; }
        public bool Inherit { get; set; }
        public bool IsInOrder { get; set; }
        public bool Required { get; set; }

        public bool CanInject(Component injectableComponet)
        {
            return IsObjectValid(injectableComponet) && IsFieldValid(injectableComponet);
        }

        public void Inject<T>(T value)
        {
            if (IsInOrder == false || value != null)
            {
                return;
            }
            IsInOrder = value == null;
            Field.SetValue(Component, value, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null);
        }

        public bool IsFieldValid(Component injectableComponet)
        {
            return
                injectableComponet.GetType() == Field.FieldType ||
                (Inherit && Field.FieldType.IsAssignableFrom(injectableComponet.GetType()));
        }

        public bool IsObjectValid(Component injectableComponet)
        {
            return
                (string.IsNullOrEmpty(From) == false && From == injectableComponet.GameObject.Name) ||
                string.IsNullOrEmpty(From) && injectableComponet.GameObject == Component.GameObject;
        }
    }
}