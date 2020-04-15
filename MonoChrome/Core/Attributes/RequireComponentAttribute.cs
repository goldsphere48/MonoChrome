using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireComponentAttribute : Attribute, IComponentValidatorAcceptable
    {
        public readonly Type RequiredComponent;
        public bool Visited => _visited;

        private bool _visited = false;

        public RequireComponentAttribute(Type componentType)
        {
            RequiredComponent = componentType;
        }

        public void AcceptComponentVisitor(ComponentAttributeVisitor visitor)
        {
            visitor.VisitRequireComponentAttribute(this);
            _visited = true;
        }
    }
}
