using System;

namespace AppGet.Exceptions
{
    public abstract class AppGetException : ApplicationException
    {
        protected AppGetException(string message)
            : base(message)
        {

        }

        protected AppGetException(string message, params object[] args)
            : base(String.Format(message, args))
        {

        }
    }
}