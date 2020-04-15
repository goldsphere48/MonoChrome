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
        private bool _visited = false;
        public bool Visited => _visited;

        public void AcceptFieldVisitor(FieldAttributeVisitor visitor)
        {
            visitor.VisitInsertComponentAttribute();
            _visited = true;
        }
    }
}
