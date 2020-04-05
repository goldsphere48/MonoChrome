using MonoChrome.GameObjectSystem.Components.Attributes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class CreatedForAttribute : Attribute, IComponentVisitorAcceptable
    {
        public readonly Type TargetType;
        public bool Inherit { get; set; } = false;

        public CreatedForAttribute(Type targetType)
        {
            TargetType = targetType;
        }

        public void AcceptVisitor(ComponentAttributeVisitor visitor)
        {
            visitor.VisitCreatedForAttribute(this);
        }
    }
}
