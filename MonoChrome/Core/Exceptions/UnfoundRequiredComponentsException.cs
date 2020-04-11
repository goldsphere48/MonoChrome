using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Exceptions
{
    class UnfoundRequiredComponentsException : Exception
    {
        public UnfoundRequiredComponentsException(string message) : base(message)
        {

        }
    }
}
