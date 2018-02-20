namespace AppGet.CommandLine.Prompts
{
    public interface IPrompt<T>
    {
        T Request(string message, T defaultValue);
    }


    public class TextPrompt : PromptBase<string>
    {

        protected override bool Convert(string input, out string result)
        {
            result = (input ?? "").Trim();
            return true;
        }
    }
}
