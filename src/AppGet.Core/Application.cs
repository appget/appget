using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppGet.Download;
using TinyIoC;

namespace AppGet
{
    public static class Application
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public static TimeSpan ApplicationLifetime
        {
            get
            {
                return Stopwatch.Elapsed;
            }
        }

        public static int Main(string[] args)
        {
            var container = new TinyIoCContainer();
            container.AutoRegister();

            RegisterDownloadClients(container);

            return 0;
        }

        private static void RegisterDownloadClients(TinyIoCContainer container)
        {
            container.RegisterMultiple<IDownloadClient>(new List<Type> { typeof(HttpDownloadClient) });
        }
    }
}