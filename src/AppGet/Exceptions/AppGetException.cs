using System;

namespace AppGet.Exceptions
{
    public abstract class AppGetException : ApplicationException
    {
        protected AppGetException(string message) : base(message)
        {

        }
    }
}