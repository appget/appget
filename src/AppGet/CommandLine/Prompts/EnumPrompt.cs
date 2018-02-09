using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGet.CommandLine.Prompts
{
    public class EnumPrompt<T> where T : struct
    {
        private readonly List<T> _members;

        public EnumPrompt()
        {
            _members = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        private void PrintOptions()
        {
            Console.WriteLine();

            for (var i = 0; i < _members.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {_members[i]}");
            }
        }


        public T? Request(string message, T defaultValue)
        {
            Console.WriteLine();

            Console.Write($"{message} (default: {defaultValue}): ");

            PrintOptions();

            var result = defaultValue;
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                result = defaultValue;
            }
            else
            {
                var parseResult = ParseEnum(input);
                if (parseResult != null)
                {
                    return parseResult.Value;
                }

                Console.WriteLine("Invalid Selection. Please select from one of the options.");
                return Request(message, defaultValue);
            }

            return result;
        }


        private T? ParseEnum(string value)
        {
            if (Int16.TryParse(value, out var index))
            {
                if (_members.Count > index)
                {
                    return _members[index - 1];
                }
            }

            foreach (var m in _members)
            {
                if (string.Equals(m.ToString(), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return m;
                }
            }

            return null;
        }
    }
}
