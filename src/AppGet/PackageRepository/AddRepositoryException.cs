using System;

namespace AppGet.PackageRepository
{
    public class AddRepositoryException : Exception
    {
        public AddRepositoryException(string message) : base($"Couldn't add repository. {message}")
        {

        }
    }
}