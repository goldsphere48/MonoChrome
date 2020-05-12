using System;

namespace MonoChrome.Core.Exceptions
{
    internal class InvalidComponentDuplicateException : Exception
    {
        public InvalidComponentDuplicateException(string message) : base(message)
        {
        }
    }
}
