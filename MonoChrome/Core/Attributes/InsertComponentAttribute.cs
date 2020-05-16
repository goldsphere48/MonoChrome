using MonoChrome.Core.Helpers.FieldInjection;
using System;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InsertComponentAttribute : Attribute, IComponentApplicatorAcceptable
    {
        public string From { get; set; }
        public bool Inherit { get; set; } = false;
        public bool Required { get; set; } = false;

        public void AcceptFieldVisitor(FieldAttributeVisitor visitor)
        {
            visitor.VisitInsertComponentAttribute(From, Inherit, Required);
        }
    }
}