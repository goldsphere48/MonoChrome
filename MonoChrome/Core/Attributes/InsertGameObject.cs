using MonoChrome.Core.Helpers.FieldInjection;
using System;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InsertGameObjectAttribute : Attribute, IComponentApplicatorAcceptable
    {
        public bool Required { get; set; }

        public InsertGameObjectAttribute(string name)
        {
            _name = name;
        }

        private string _name;

        public void AcceptFieldVisitor(FieldAttributeVisitor visitor)
        {
            visitor.VisitInsertGameObjectAttribute(_name, Required);
        }
    }
}