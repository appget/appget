using AppGet.Exceptions;

namespace AppGet.Crypto.Hash
{
    public class ChecksumVerificationException : AppGetException
    {
        public ChecksumVerificationException(string message) : base(message)
        {
        }
    }
}