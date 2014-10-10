using AppGet.Exceptions;

namespace AppGet.FileTransfer
{
    public class ProtocolNotSupportedException : AppGetException
    {
        public ProtocolNotSupportedException(string message, params object[] args)
            : base(message, args)
        {
        }
    }
}
