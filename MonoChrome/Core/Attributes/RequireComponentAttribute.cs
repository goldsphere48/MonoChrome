using MonoChrome.Core.Helpers.ComponentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireComponentAttribute : Attribute, IComponentVisitorAcceptable
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
