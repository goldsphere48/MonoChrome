using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes.Helpers
{
    interface IComponentVisitorAcceptable
    {
        void AcceptVisitor(ComponentAttributeVisitor visitor);
    }
}
