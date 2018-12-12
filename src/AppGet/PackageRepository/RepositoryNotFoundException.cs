using System;

namespace AppGet.PackageRepository
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException(string name = null, string id = null)
            : base($"Couldn't find a repository matching Name: {name}, ID: {id}")
        {
        }
    }
}