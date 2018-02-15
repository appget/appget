using System;

namespace AppGet.CommandLine.Prompts
{
    public abstract class PromptBase<T> : IPrompt<T>
    {
        public T Request(string message, T defaultValue)
        {
            Console.WriteLine();

            T result;

            Console.WriteLine(GetPromptText(message, defaultValue));

            PrintHints();


            var inputString = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(inputString) && defaultValue != null)
            {
                return defaultValue;
            }


            if (!TryParse(inputString, out result))
            {
                result = Request(message, defaultValue);
            }

            return result;
        }

        protected abstract bool TryParse(string input, out T result);

        protected virtual string GetPromptText(string message, T defaultValue)
        {
            if (defaultValue != null)
            {
                return ($"{message} (guess: {defaultValue.ToString().Trim()}): ");
            }

            return $"{message}: ";
        }

        protected virtual void PrintHints()
        {
        }
    }
}