using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace AppGet.PackageRepository
{
    public class RepositoryRegistry
    {
        private const string SOFTWARE_APPGET_REPOSITORIES = @"SOFTWARE\AppGet\Repositories";

        public void AddRepo(Repository repo)
        {
            if (All().Any(c => string.Equals(c.Name, repo.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new AddRepositoryException($"Repository with the name [{repo.Name}] already exists.");
            }

            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                {
                    using (var repoKey = baseKey.CreateSubKey($"{SOFTWARE_APPGET_REPOSITORIES}\\{repo.Name}", RegistryKeyPermissionCheck.ReadWriteSubTree))
                    {
                        repoKey.SetValue("Connection", repo.Connection);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new AddRepositoryException("Administrative permissions are required to add a repository.");
            }
        }

        public void Remove(string name)
        {
            var repo = Get(name);
            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                localMachine.DeleteSubKey($"{SOFTWARE_APPGET_REPOSITORIES}\\{repo.Name}", true);
            }
        }

        public Repository Get(string name)
        {
            return All().First(c => c.Name.ToLower() == name.ToLowerInvariant());
        }

        public IEnumerable<Repository> All()
        {
            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                using (var repositoryRoot = baseKey.OpenSubKey(SOFTWARE_APPGET_REPOSITORIES, RegistryKeyPermissionCheck.ReadSubTree))
                {
                    if (repositoryRoot == null) yield break;

                    foreach (var subKey in repositoryRoot.GetSubKeyNames())
                    {
                        using (var repoKey = repositoryRoot.OpenSubKey(subKey))
                        {
                            yield return new Repository
                            {
                                Name = subKey,
                                Connection = repoKey.GetValue("Connection").ToString()
                            };
                        }
                    }
                }
            }
        }
    }
}
