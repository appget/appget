using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WindowsInput;

namespace AppGet.CommandLine.Prompts
{
    public abstract class PromptBase<T> : IPrompt<T>
    {
        private readonly IKeyboardSimulator _keyboardSimulator = new InputSimulator().Keyboard;

        public T Request(string message, T defaultValue)
        {
            PrintPrompt(message, defaultValue);

            var inputString = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(inputString))
            {
                return default(T);
            }


            if (!TryParse(inputString, out var result))
            {
                Console.WriteLine($"'{inputString}' is not a valid entry.");
                result = Request(message, defaultValue);
            }

            return result;
        }

        private void PrintPrompt(string message, T defaultValue)
        {
            var options = Options;

            Console.WriteLine();
            if (options.Any())
            {
                Console.WriteLine(message);
                for (var i = 1; i <= options.Count; i++)
                {
                    Console.WriteLine($" {i}: {OptionString(options[i - 1]),3}");
                }

                Console.WriteLine();
                Console.Write("Selection: ");
            }
            else
            {
                Console.Write($"{message}: ");
            }

            if (defaultValue != null)
            {
                var defaultString = OptionString(defaultValue);
                if (!string.IsNullOrWhiteSpace(defaultString))
                {
                    Thread.Sleep(50);
                    _keyboardSimulator.TextEntry(defaultString.Trim());
                }
            }
        }

        private bool TryParse(string input, out T result)
        {
            var options = Options;
            input = input.Trim();

            result = default(T);

            if (options.Any())
            {
                if (int.TryParse(input, out int index))
                {
                    if (index > 0 && index - 1 < options.Count)
                    {
                        result = options[index - 1];
                        return true;
                    }
                }
                else
                {
                    var found = options.Any(c => string.Equals(OptionString(c), input, StringComparison.CurrentCultureIgnoreCase));
                    if (found)
                    {
                        result = options.First(c => string.Equals(OptionString(c), input, StringComparison.CurrentCultureIgnoreCase));
                    }

                    return found;
                }
            }

            return Convert(input, out result);
        }

        protected virtual string OptionString(T option)
        {
            return option.ToString().Trim();
        }


        protected abstract bool Convert(string input, out T result);

        protected virtual List<T> Options => new List<T>();
    }
}