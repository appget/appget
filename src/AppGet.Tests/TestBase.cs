using AutoMoq;
using NUnit.Framework;

namespace AppGet.Tests
{
    public abstract class TestBase<T>
    {

        protected AutoMoqer Mocker { get; private set; }

        [SetUp]
        public void BaseSetup()
        {
            Mocker = new AutoMoqer();
        }

        public T Subject
        {
            get
            {
                return Mocker.Resolve<T>();
            }
        }

    }
}