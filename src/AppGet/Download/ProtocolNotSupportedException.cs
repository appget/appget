using System;

namespace AppGet.Download
{
    public class ProtocolNotSupportedException : Exception
    {
        public ProtocolNotSupportedException(string message, params object[] args)
            : base(String.Format(message, args))
        {
        }
    }
}
