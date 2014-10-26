using System.Text;

namespace AppGet.Options
{
    public class HelpGenerator
    {
        public string GenerateHelp()
        {
            return new RootOptions().GetUsage();
        }

        public string GenerateHelp(AppGetOption option)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("AppGet Version: 1.0");

            stringBuilder.AppendLine(option.GetUsage());

            return stringBuilder.ToString();
        }
    }
}