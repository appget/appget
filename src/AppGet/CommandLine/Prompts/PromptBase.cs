using System;

namespace AppGet.CommandLine.Prompts
{
    public abstract class PromptBase<T> : IPrompt<T>
    {
        public T Request(string message, T defaultValue)
        {
            Console.WriteLine();

            T result;

            if (defaultValue != null)
            {
                Console.Write($"{message} (guess: {defaultValue.ToString().Trim()}): ");
            }
            else
            {
                Console.Write($"{message}: ");
            }

            PrintHints();


            var inputString = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(inputString))
            {
                result = defaultValue;
            }


            if (!TryParse(inputString, out result))
            {
                result = Request(message, defaultValue);
            }

            return result;
        }

        protected abstract bool TryParse(string input, out T result);

        protected virtual void PrintHints()
        {
        }
    }
}