using System;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateHomePage : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;


        public PopulateHomePage(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifestBuilder, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (manifestBuilder.Home.Top != null) return;

            manifestBuilder.Home.Add(HomepageParser.Parse(manifestBuilder.Url), Confidence.Low, this);


            if (interactive && !manifestBuilder.Home.HasConfidence(Confidence.VeryHigh))
            {
                var result = _prompt.Request("Product Homepage", manifestBuilder.Home.Top);
                manifestBuilder.Home.Add(result, Confidence.VeryHigh, this);
            }
        }
    }
}

