using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.GameObjectSystem.Components.Attributes.Helpers
{
    class ComponentAttributesInfo
    {
        public bool AllowMultipleComponentUsage { get; set; } = true;
        public Type TargetType { get; set; } = null;
        public bool TargetTypeInherit { get; set; } = false;
        public List<Type> RequiredComponents { get; } = new List<Type>();
    }
}
