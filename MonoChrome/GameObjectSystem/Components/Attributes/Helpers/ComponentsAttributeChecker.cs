using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes.Helpers;
using MonoChrome.GameObjectSystem.Components.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes
{
    static class ComponentsAttributeChecker
    {
        private static ComponentAttributesInfo GetAttributesInfo(Component component)
        {
            var componentTypeInfo = component.GetType();
            var componentAttributes = componentTypeInfo.GetCustomAttributes(false);
            var attributeVisitor = new ComponentAttributeVisitor();
            foreach (var componentAttribute in componentAttributes)
            {
                if (componentAttribute is IComponentVisitorAcceptable)
                {
                    var acceplatbleAttribute = componentAttribute as IComponentVisitorAcceptable;
                    acceplatbleAttribute.AcceptVisitor(attributeVisitor);
                }
            }
            return attributeVisitor.Result;
        }

        public static bool Verify(Component component, GameObject gameObject)
        {
            var componentsInfo = GetAttributesInfo(component);
            CheckAllowMultipleComponentUsage(component, gameObject, componentsInfo);
            CheckUnfoundedComponents(gameObject, componentsInfo);
            CheckTargetType(component, gameObject, componentsInfo);
            return true;
        }

        private static bool CheckAllowMultipleComponentUsage(Component component, GameObject gameObject, ComponentAttributesInfo componentsInfo)
        {
            if (!componentsInfo.AllowMultipleComponentUsage)
            {
                if (gameObject.GetComponent(component.GetType()) != null)
                {
                    throw new InvalidComponentDuplicateException(component.GetType());
                }
            }
            return true;
        }

        private static bool CheckUnfoundedComponents(GameObject gameObject, ComponentAttributesInfo componentsInfo)
        {
            var unfoundedRequiredComponents = new List<Type>();
            foreach (var requiredComponent in componentsInfo.RequiredComponents)
            {
                if (gameObject.GetComponent(requiredComponent.GetType()) == null)
                {
                    unfoundedRequiredComponents.Add(requiredComponent);
                }
            }
            if (unfoundedRequiredComponents.Count > 0)
            {
                throw new UnfoundRequiredComponentsException(unfoundedRequiredComponents);
            }
            return true;
        }

        private static bool CheckTargetType(Component component, GameObject gameObject, ComponentAttributesInfo componentsInfo)
        {
            bool inherit = componentsInfo.TargetTypeInherit;
            bool isSameOrSubclass = componentsInfo.TargetType.IsAssignableFrom(gameObject.GetType());
            bool isSame = componentsInfo.TargetType != gameObject.GetType();
            bool notCorrect = (inherit && !isSameOrSubclass) ||
                              (!inherit && !isSame);
            if (notCorrect)
            {
                throw new InvalidComponentTargetException(component.GetType(), gameObject.GetType());
            }
            return true;
        }
    }
}
