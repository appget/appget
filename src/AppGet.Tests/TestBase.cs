using AutoMoq;
using NLog;
using NUnit.Framework;

namespace AppGet.Tests
{
    public abstract class TestBase<T> where T : class
    {
        protected AutoMoqer Mocker { get; private set; }

        private T _subject = null;

        protected Logger logger = LogManager.GetLogger("logger");

        [SetUp]
        public void BaseSetup()
        {
            _subject = null;
            Mocker = new AutoMoqer();
        }

        public T Subject
        {
            get
            {
                if (_subject == null)
                {
                    _subject = Mocker.Resolve<T>();
                }

                return _subject;
            }
        }

    }
}
