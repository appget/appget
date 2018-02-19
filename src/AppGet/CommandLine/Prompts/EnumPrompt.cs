using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGet.CommandLine.Prompts
{
    public class EnumPrompt<T> : PromptBase<T> where T : struct
    {
        protected override bool Convert(string input, out T result)
        {
            throw new NotImplementedException();
        }

        protected override List<T> Options => Enum.GetValues(typeof(T)).Cast<T>().ToList();

    }
}
