using System;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CommandLine.Prompts
{
    public class WindowsVersionPrompt : PromptBase<Version>
    {
        protected override void PrintHints()
        {
            Console.WriteLine();

            var versions = WindowsVersion.KnownVersions.ToList();

            for (var i = 0; i < versions.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {WindowsVersion.ToDesktopName(versions[i])}");
            }
        }

        protected override bool TryParse(string input, out Version result)
        {
            var versions = WindowsVersion.KnownVersions.ToList();

            if (Int16.TryParse(input, out var index))
            {
                if (versions.Count >= index)
                {
                    result = versions[index - 1];
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
