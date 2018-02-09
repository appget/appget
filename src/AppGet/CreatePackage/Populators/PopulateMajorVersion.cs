using System;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateMajorVersion : IPopulateManifest
    {
        private readonly IPrompt _prompt;

        public PopulateMajorVersion(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var major = manifest.Version.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            manifest.MajorVersion = _prompt.Request("Major version", major).ToLowerInvariant();
        }
    }
}