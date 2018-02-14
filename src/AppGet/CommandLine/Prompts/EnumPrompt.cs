using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGet.CommandLine.Prompts
{
    public class EnumPrompt<T> : PromptBase<T> where T : struct
    {
        private readonly List<T> _members;

        public EnumPrompt()
        {
            _members = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        protected override void PrintHints()
        {
            Console.WriteLine();

            for (var i = 0; i < _members.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {_members[i]}");
            }
        }

        protected override bool TryParse(string input, out T result)
        {
            if (Int16.TryParse(input, out var index))
            {
                if (_members.Count >= index)
                {
                    result = _members[index - 1];
                    return true;
                }
            }

            foreach (var m in _members)
            {
                if (string.Equals(m.ToString(), input, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = m;
                    return true;
                }
            }

            result = default(T);
            return false;
        }
    }
}
