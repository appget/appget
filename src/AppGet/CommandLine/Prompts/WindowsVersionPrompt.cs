using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifest;

namespace AppGet.CommandLine.Prompts
{
    public class WindowsVersionPrompt : PromptBase<Version>
    {
        protected override List<Version> Options => WindowsVersion.KnownVersions.ToList();

        protected override bool Convert(string input, out Version result)
        {
            result = null;
            return false;
        }

        protected override string OptionString(Version option)
        {
            return $"{WindowsVersion.ToDesktopName(option)} / {WindowsVersion.ToServerName(option)}";
        }
    }
}