using System;

namespace AppGet.PackageRepository
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException(string name) : base($"Couldn't find a repository matching name: {name}")
        {
        }
    }
}