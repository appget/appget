using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AppGet.Tools
{
    public interface IPeManifestReader
    {
        string Read(string path);
    }

    public class PeManifestReader : IPeManifestReader
    {
        public string Read(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentOutOfRangeException(path);

            if (!File.Exists(path))
                throw new FileNotFoundException("Parameter \"path\" does not point to a existing file");


            var systemDeploymentAssembly = Assembly.Load("System.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            var systemUtilsClass = systemDeploymentAssembly.GetType("System.Deployment.Application.Win32InterOp.SystemUtils");
            var systemUtils = Activator.CreateInstance(systemUtilsClass);

            //Invoke the internal method named "GetManifestFromPEResources" via reflection
            var manifestBytes = systemUtils.GetType().InvokeMember(
                "GetManifestFromPEResources",
                BindingFlags.InvokeMethod |
                BindingFlags.Public |
                BindingFlags.Static,
                null,
                systemUtils,
                new object[] { path }) as byte[];

            if (manifestBytes == null) return null;

            //Read bytes with memory stream and stream reader to make sure
            //to get the right encoded data, because some of the resources do have a BOM (Byte Order Mark)
            //Read this for more information: http://en.wikipedia.org/wiki/Byte_Order_Mark

            using (var manifestBytesMemoryStream = new MemoryStream(manifestBytes))
            using (var manifestBytesStreamReader = new StreamReader(manifestBytesMemoryStream, true))
            {
                return manifestBytesStreamReader.ReadToEnd().Trim();
            }

        }
    }
}
