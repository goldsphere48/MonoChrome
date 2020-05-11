using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InsertComponentAttribute : Attribute, IComponentApplicatorAcceptable
    {
        public string From { get; set; }
        public bool Inherit { get; set; } = false;

        public void AcceptFieldVisitor(FieldAttributeVisitor visitor)
        {
            visitor.VisitInsertComponentAttribute(From, Inherit);
        }
    }
}
