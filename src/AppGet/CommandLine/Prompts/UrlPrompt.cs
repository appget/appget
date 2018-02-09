using System;

namespace AppGet.CommandLine.Prompts
{
    public interface IUrlPrompt : IPrompt
    {
    }

    public class UrlPrompt : Prompt, IUrlPrompt
    {

        protected override bool IsValid(string value)
        {
            var isValid = Uri.IsWellFormedUriString(value, UriKind.Absolute);

            if (!isValid)
            {
                Console.WriteLine($"'{value}' is not a valid URL.");
            }

            return isValid;
        }
    }
}