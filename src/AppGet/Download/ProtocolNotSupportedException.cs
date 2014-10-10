using System;
using AppGet.Exceptions;

namespace AppGet.Download
{
    public class ProtocolNotSupportedException : AppGetException
    {
        public ProtocolNotSupportedException(string message, params object[] args)
            : base(message, args)
        {
        }
    }
}
