using System;
using System.Collections.Generic;

namespace AppGet.CommandLine.Prompts
{
    public class BooleanPrompt : PromptBase<Boolean>
    {
        private static readonly HashSet<string> Yes = new HashSet<string>
        {
            "true",
            "yes",
            "y"
        };

        private static readonly HashSet<string> No = new HashSet<string>
        {
            "false",
            "no",
            "n"
        };

        protected override bool Convert(string input, out bool result)
        {
            input = input.Trim().ToLowerInvariant();

            if (Yes.Contains(input))
            {
                result = true;

                return true;
            }

            if (No.Contains(input))
            {
                result = false;

                return true;
            }

            result = false;

            return false;
        }

        protected override string OptionString(bool option)
        {
            return option ? "yes" : "no";
        }
    }
}