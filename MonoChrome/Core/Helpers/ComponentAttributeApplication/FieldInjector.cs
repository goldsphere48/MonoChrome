using MonoChrome.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    internal class FieldInjector
    {
        private delegate string IssueTemplate(Component component, DependencyFieldInfo info);
        private FieldAttributeVisitor _fieldAttributeVisitor = new FieldAttributeVisitor();

        public IEnumerable<AttributeError> GetIssues(Component component)
        {
            return GetIssuses(component, _fieldAttributeVisitor.DependencyComponents, ComponentIssueTemplate)
                .Concat(GetIssuses(component, _fieldAttributeVisitor.DependencyObjects, GameObjectIssueTemplate));
        }

        public void OnComponentAdded(Component component)
        {
            RegistryFields(component);
            InjectComponent(component);
        }

        public void OnComponentRemove(Component component)
        {
            InjectNull(component);
            EraseFields(component);
        }

        public void OnObjectEntered(GameObject gameObject)
        {
            InjectGameObject(gameObject);
        }

        public void OnObjectDrop(GameObject gameObject)
        {
            InjectNull(gameObject);
        }

        private IEnumerable<AttributeError> GetIssuses(Component component, IEnumerable<DependencyFieldInfo> dependencyList, IssueTemplate template)
        {
            return from info in _fieldAttributeVisitor.DependencyObjects
                   where info.IsInOrder && info.Required
                   select new AttributeError { Message = template?.Invoke(component, info) };
        }

        private string ComponentIssueTemplate(Component component, DependencyFieldInfo info)
        {
            return $"Can't find value for field {info.Field.Name} in component {info.Component.GetType().Name} of object {component.GameObject.Name}";
        }

        private string GameObjectIssueTemplate(Component component, DependencyFieldInfo info)
        {
            return $"Can't find game object with name {info.From} for field {info.Field.Name} in component {component.GetType().Name} of object {component.GameObject.Name}";
        }

        private void RegistryFields(Component component)
        {
            Console.WriteLine(component);
            _fieldAttributeVisitor.CurrentComponent = component;
            var fields = GetAllFields(component.GetType());
            foreach (var field in fields)
            {
                ProceedField(field);
            }
        }

        private void EraseFields(Component component)
        {
            _fieldAttributeVisitor.DependencyComponents.RemoveAll(e => e.Component == component);
            _fieldAttributeVisitor.DependencyObjects.RemoveAll(e => e.Component == component);
        }

        private void InjectComponent(Component injectableComponent, bool injectNull = false)
        {
            foreach (var info in _fieldAttributeVisitor.DependencyComponents)
            {
                if (info.CanInject(injectableComponent))
                {
                    info.Inject(injectNull ? null : injectableComponent);
                }
            }
        }

        private void InjectGameObject(GameObject gameObject, bool injectNull = false)
        {
            foreach (var info in _fieldAttributeVisitor.DependencyObjects)
            {
                if (info.From == gameObject.Name)
                {
                    info.Inject(injectNull ? null : gameObject);
                }
            }
        }

        private void InjectNull(Component injectableComponent)
        {
            InjectComponent(injectableComponent, true);
        }

        private void InjectNull(GameObject injectableObject)
        {
            InjectGameObject(injectableObject, true);
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