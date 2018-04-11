using AppGet.Extensions;

namespace AppGet.CommandLine.Prompts
{
    public interface IPrompt<T>
    {
        T Request(string message, T defaultValue);
    }

    public class TextPrompt : PromptBase<string>
    {
        public bool Required { get; set; }

        protected override bool Convert(string input, out string result)
        {
            if (Required && input.IsNullOrWhiteSpace())
            {
                result = null;

                return false;
            }

            result = (input ?? "").Trim();

            return true;
        }
    }
}