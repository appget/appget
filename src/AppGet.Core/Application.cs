using System;
using System.Diagnostics;
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
            return 0;
        }
    }
}