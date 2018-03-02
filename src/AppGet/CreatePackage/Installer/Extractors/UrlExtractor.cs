using System;
using AppGet.CreatePackage.Parsers;

namespace AppGet.CreatePackage.Installer.Extractors
{
    public class UrlExtractor : IExtractToInstaller
    {
        public void Extract(InstallerBuilder installer)
        {
            installer.Architecture.Add(ArchitectureParser.Parse(new Uri(installer.Location)), Confidence.Plausible, this);
        }
    }
}