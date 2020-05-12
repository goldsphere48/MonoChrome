using MonoChrome.Core.Helpers.ComponentAttributeApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InsertGameObjectAttribute : Attribute, IComponentApplicatorAcceptable
    {
        public bool Required { get; set; }
        private string _name;
        public InsertGameObjectAttribute(string name)
        {
            _name = name;
        }

        public void AcceptFieldVisitor(FieldAttributeVisitor visitor)
        {
            visitor.VisitInsertGameObjectAttribute(_name, Required);
        }
    }
}
