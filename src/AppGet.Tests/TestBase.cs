using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Http;
using AppGet.Infrastructure.Logging;
using AutoMoq;
using NLog;
using NUnit.Framework;

namespace AppGet.Tests
{
    public abstract class TestBase<T> where T : class
    {
        protected AutoMoqer Mocker { get; private set; }

        private T _subject;

        protected Logger logger = LogManager.GetLogger("logger");

        [SetUp]
        public void BaseSetup()
        {
            _subject = null;
            Mocker = new AutoMoqer();
            Mocker.SetInstance(logger);

            LogConfigurator.EnableConsoleTarget(LogConfigurator.DetailedLayout, LogLevel.Trace);
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
            Mocker.SetInstance<IEnvInfo>(Mocker.Resolve<EnvInfo>());
            Mocker.SetInstance<IUserAgentBuilder>(Mocker.Resolve<UserAgentBuilder>());
            Mocker.SetInstance(Mocker.Resolve<MachineId>());
            Mocker.SetInstance<IHttpClient>(Mocker.Resolve<HttpClient>());
        }

        protected void WithRealFileSystem()
        {
            Mocker.SetInstance<IFileSystem>(Mocker.Resolve<FileSystem.FileSystem>());
        }

        protected string GetTestPath(string path)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, Path.Combine(path.Split('/')));
        }

        protected string ReadAllText(string path)
        {
            return File.ReadAllText(GetTestPath(path));
        }
    }
}
