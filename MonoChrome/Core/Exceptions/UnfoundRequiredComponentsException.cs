using System;

namespace MonoChrome.Core.Exceptions
{
    internal class UnfoundRequiredComponentsException : Exception
    {
        public UnfoundRequiredComponentsException(string message) : base(message)
        {
        }
    }
}
