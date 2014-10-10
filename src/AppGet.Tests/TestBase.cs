using AutoMoq;
using NLog;
using NUnit.Framework;

namespace AppGet.Tests
{
    public abstract class TestBase<T>
    {

        protected AutoMoqer Mocker { get; private set; }

        protected Logger logger = LogManager.GetLogger("logger"); 

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