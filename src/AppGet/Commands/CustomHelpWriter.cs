using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AppGet.Extensions;
using Colorful;

namespace AppGet.Commands
{
    public class CustomHelpWriter : TextWriter
    {
        public override Encoding Encoding { get; }

        public override void Write(string value)
        {
            var lines = Regex.Split(value, "\r\n|\r|\n");

            if (lines.Length < 3)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }

                return;
            }

            foreach (var line in lines)
            {
                if (line.StartsWith("Copyright")) continue;
                if (line.StartsWith("AppGet ")) continue;

                if (line.StartsWith("ERROR"))
                {
                    Console.WriteLine("");
                    continue;
                }

                if (line.StartsWith("  ") || line.IsNullOrWhiteSpace())
                {
                    Console.WriteLine(line);
                    continue;
                }

                Console.WriteLine(line, Color.Red);
            }
        }
    }
}