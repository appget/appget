using System;

namespace AppGet.CommandLine.Prompts
{
    public interface IUrlPrompt : IPrompt<string>
    {
    }

    public class UrlPrompt : TextPrompt, IUrlPrompt
    {
        protected override bool Convert(string input, out string result)
        {
            var isValid = Uri.IsWellFormedUriString(input, UriKind.Absolute);

            if (!isValid)
            {
                Console.WriteLine($"'{input}' is not a valid URL.");
            }

            result = input.Trim();

            return isValid;

        }
    }
}