using MonoChrome.GameObjectSystem.Components.Attributes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class ComponentUsageAttribute : Attribute, IComponentVisitorAcceptable
    {
        public bool AllowMultipleComponentUsage { get; set; } = true;

        public void AcceptVisitor(ComponentAttributeVisitor visitor)
        {
            visitor.VisitComponentUsageAttribute(this);
        }
    }
}
