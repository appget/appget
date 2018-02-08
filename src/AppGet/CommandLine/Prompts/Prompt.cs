using System;

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
            //            while (true)
            //            {
            if (defaultValue != null)
            {
                message = $"{message} (default: '{defaultValue}')";
            }

            Console.WriteLine(message);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                input = defaultValue;
            }

            return input?.Trim();


            //                if (validator == null || validator(input))
            //                {
            //                    return input?.Trim();
            //                }

            //                Console.WriteLine("Invalid input");
            //            }
        }
    }
}
