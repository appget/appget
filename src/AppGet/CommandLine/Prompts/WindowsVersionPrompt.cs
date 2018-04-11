using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CommandLine.Prompts
{
    public class WindowsVersionPrompt : PromptBase<Version>
    {
        protected override List<Version> Options => WindowsVersion.KnownVersions.ToList();

        protected override bool Convert(string input, out Version result)
        {
            throw new NotImplementedException();
        }

        protected override string OptionString(Version option)
        {
            return WindowsVersion.ToDesktopName(option);
        }
    }
}