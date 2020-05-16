using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    internal class ComponentInjector
    {
        private FieldAttributeVisitor _fieldAttributeVisitor = new FieldAttributeVisitor();

        public void RegistryFields(Component component)
        {
            _fieldAttributeVisitor.CurrentComponent = component;
            var fields = GetAllFields(component.GetType());
            foreach (var field in fields)
            {
                ProceedField(field);
            }
        }

        public void OnComponentAdded(Component component)
        {
            InjectComponent(component, component);
        }

        public void OnComponentRemove(Component component)
        {
            InjectComponent(component, null);
        }

        public void OnObjectEntered(GameObject gameObject)
        {
            InjectGameObject(gameObject, gameObject);
        }

        public void OnObjectDrop(GameObject gameObject)
        {
            InjectGameObject(gameObject, null);
        }

        private bool IsObjectValid(Component component, DependencieFieldInfo info)
        {
            return
                (!string.IsNullOrEmpty(info.From) && info.From == component.GameObject.Name) ||
                component.GameObject.Name == info.Component.GameObject.Name;
        }

        private bool IsFieldValid(Component component, DependencieFieldInfo info)
        {
            return
                component.GetType() == info.Field.FieldType ||
                (info.Inherit && info.Field.FieldType.IsAssignableFrom(component.GetType()));
        }

        private bool CanInject(Component component, DependencieFieldInfo info)
        {
            return IsObjectValid(component, info) && IsFieldValid(component, info);
        }

        private void InjectComponent(Component component, Component value)
        {
            foreach (var info in _fieldAttributeVisitor.DependencieComponents)
            {
                if (CanInject(component, info))
                {
                    info.Inject(value);
                }
            }
        }

        private void InjectGameObject(GameObject gameObject, GameObject value)
        {
            foreach (var info in _fieldAttributeVisitor.DependencieObjects)
            {
                if (info.From == gameObject.Name)
                {
                    info.Inject(gameObject);
                }
            }
        }

        private IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            if (type == null && !typeof(Component).IsAssignableFrom(type))
            {
                return Enumerable.Empty<FieldInfo>();
            }
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Concat(GetAllFields(type.BaseType));
        }
        private void ProceedField(FieldInfo field)
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
        private void ProceedFieldAttribute(IComponentApplicatorAcceptable attribute)
        {
            attribute.AcceptFieldVisitor(_fieldAttributeVisitor);
        }
    }
}