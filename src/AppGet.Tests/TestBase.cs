using AppGet.Http;
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
            Mocker.SetInstance(logger);
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



        protected void WithRealHttp()
        {
            Mocker.SetInstance<IHttpClient>(new HttpClient(logger));
        }

    }
}
