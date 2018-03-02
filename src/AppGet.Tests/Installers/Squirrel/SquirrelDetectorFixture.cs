using AppGet.Compression;
using AppGet.Installers.Squirrel;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Squirrel
{
    [TestFixture(Category = "Local")]
    public class SquirrelDetectorFixture : DetectorTestBase<SquirrelDetector>
    {
        [SetUp]
        public void Setup()
        {
            Mocker.SetInstance<ISfxReader>(new SfxReader());
            Mocker.SetInstance<ISquirrelReader>(new SquirrelReader());
        }
    }
}