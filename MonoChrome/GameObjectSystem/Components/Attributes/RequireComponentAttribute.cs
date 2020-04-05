using MonoChrome.GameObjectSystem.Components.Attributes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class RequireComponentAttribute : Attribute, IComponentVisitorAcceptable
    {
        public readonly Type RequiredComponent;
        public RequireComponentAttribute(Type componentType)
        {
            RequiredComponent = componentType;
        }

        public void AcceptVisitor(ComponentAttributeVisitor visitor)
        {
            visitor.VisitRequireComponentAttribute(this);
        }
    }
}
