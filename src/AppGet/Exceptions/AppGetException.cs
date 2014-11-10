using System;

namespace AppGet.Exceptions
{
    public class AppGetException : ApplicationException
    {
        public AppGetException(string message)
            : base(message)
        {

        }

        public AppGetException(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}