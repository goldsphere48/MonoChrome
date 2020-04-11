using MonoChrome.Core.Helpers.ComponentValidation;
using System;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentUsageAttribute : Attribute, IComponentVisitorAcceptable
    {
        public bool AllowMultipleComponentUsage { get; set; } = true;

        public void AcceptVisitor(ComponentAttributeVisitor visitor)
        {
            visitor.VisitComponentUsageAttribute(this);
        }
    }
}
