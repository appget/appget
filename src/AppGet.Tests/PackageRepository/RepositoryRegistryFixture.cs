using System;
using System.Linq;
using AppGet.PackageRepository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.PackageRepository
{
    [TestFixture]
    public class RepositoryRegistryFixture : TestBase<RepositoryRegistry>
    {
        [Test]
        public void add_repo()
        {
            var repo = new Repository
            {
                Name = Guid.NewGuid().ToString(),
                Connection = "repourl"
            };
            Subject.AddRepo(repo);


            Subject.All().Should().Contain(c => c.Name == repo.Name && c.Connection == repo.Connection);
        }


        [Test]
        public void should_get_all()
        {
            var c = Subject.All();

            c.ToList().Should().NotBeNull();
        }

        [Test]
        public void delete_repo()
        {
            var repo = new Repository
            {
                Name = Guid.NewGuid().ToString(),
                Connection = "repourl"
            };

            Subject.AddRepo(repo);
            Subject.Remove(repo.Name.ToUpper());
        }
    }
}