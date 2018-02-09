using System;
using System.Linq;

namespace AppGet.CommandLine.Prompts
{
    public interface IPrompt
    {
        string Request(string message, string defaultValue);
    }

    public class Prompt : IPrompt
    {
        public string Request(string message, string defaultValue)
        {
            Console.WriteLine();

            if (defaultValue != null)
            {
                Console.Write($"{message} (default: {defaultValue}): ");
            }
            else
            {
                Console.Write($"{message}: ");
            }


            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                input = defaultValue?.ToString();
            }


            if (!IsValid(input))
            {
                input = Request(message, defaultValue);
            }

            return input?.Trim();
        }


        protected virtual object AutoFix<T>(string value)
        {
            return value;
        }

        protected virtual bool IsValid(string value)
        {
            return true;
        }
    }
}
