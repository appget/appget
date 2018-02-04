using System;

namespace AppGet.Exceptions
{
    public class AppGetException : ApplicationException
    {
        public AppGetException(string message)
            : base(message)
        {

        }
    }
}