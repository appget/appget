using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Http;
using AppGet.Infrastructure.Logging;
using Moq.AutoMock;
using NLog;
using NUnit.Framework;

namespace AppGet.Tests
{
    public abstract class TestBase<T> where T : class
    {
        protected AutoMocker Mocker { get; private set; }

        private T _subject;

        protected Logger logger = LogManager.GetLogger("logger");

        [SetUp]
        public void BaseSetup()
        {
            _subject = null;
            Mocker = new AutoMocker();
            Mocker.Use(logger);

            LogConfigurator.EnableConsoleTarget(LogConfigurator.DetailedLayout, LogLevel.Trace);
        }

        public T Subject
        {
            get
            {
                if (_subject == null)
                {
                    _subject = Mocker.CreateInstance<T>();
                }

                return _subject;
            }
        }



        protected void WithRealHttp()
        {
            Mocker.Use<IEnvInfo>(Mocker.CreateInstance<EnvInfo>());
            Mocker.Use<IUserAgentBuilder>(Mocker.CreateInstance<UserAgentBuilder>());
            Mocker.Use(Mocker.CreateInstance<MachineId>());
            Mocker.Use<IHttpClient>(Mocker.CreateInstance<HttpClient>());
        }

        protected void WithRealFileSystem()
        {
            Mocker.Use<IFileSystem>(Mocker.CreateInstance<FileSystem.FileSystem>());
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
