using System.Linq;

namespace AppGet.CommandLine.Prompts
{
    public interface IPrompt<T>
    {
        T Request(string message, T defaultValue);
    }


    public class TextPrompt : PromptBase<string>
    {
        protected override bool TryParse(string input, out string result)
        {
            result = input;
            return true;
        }
    }
}
