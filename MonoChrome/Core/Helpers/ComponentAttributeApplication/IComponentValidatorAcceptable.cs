using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Helpers.ComponentAttributeApplication
{
    interface IComponentValidatorAcceptable
    {
        void AcceptComponentVisitor(ComponentAttributeVisitor visitor);
        bool Visited { get; }
    }
}
