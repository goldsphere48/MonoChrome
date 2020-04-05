using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes.Helpers
{
    class ComponentAttributeVisitor
    {
        private ComponentAttributesInfo _result = new ComponentAttributesInfo();

        public ComponentAttributesInfo Result => _result;

        public void VisitComponentUsageAttribute(ComponentUsageAttribute attribute)
        {
            _result.AllowMultipleComponentUsage = attribute.AllowMultipleComponentUsage;
        }

        public void VisitRequireComponentAttribute(RequireComponentAttribute attribute)
        {
            _result.RequiredComponents.Add(attribute.RequiredComponent);
        }
    }
}
