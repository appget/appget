using System;
using System.Collections.Generic;
using AppGet.Download;
using TinyIoC;

namespace AppGet.Composition
{
    public static class ContainerBuilder
    {
        public static TinyIoCContainer Build()
        {
            var container = new TinyIoCContainer();
            RegisterDownloadClients(container);

            return container;
        }


        private static void RegisterDownloadClients(TinyIoCContainer container)
        {
            container.RegisterMultiple<IDownloadClient>(new List<Type> { typeof(HttpDownloadClient) });
        }
    }
}