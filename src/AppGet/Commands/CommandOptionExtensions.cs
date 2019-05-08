using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppGet.Installers;

namespace AppGet.Commands
{
    public static class CommandOptionExtensions
    {
        public static InstallInteractivityLevel GetInteractivityLevel(this IVariableInteractivityCommand options)
        {
            if (options.Interactive) return InstallInteractivityLevel.Interactive;
            if (options.Silent) return InstallInteractivityLevel.Silent;
            return InstallInteractivityLevel.Passive;
        }


        public static Dictionary<string, string> ToDictionary(this ICommandLineOption command, string prefix = "cmd_")
        {
            string GetKey(string key)
            {
                return $"{prefix}{key}";
            }


            var c = command.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => GetKey(prop.Name.ToLowerInvariant()), prop => prop.GetValue(command, null)?.ToString());

            if (command is IVariableInteractivityCommand interactivityCommand)
            {
                c[GetKey("interactivity")] = GetInteractivityLevel(interactivityCommand).ToString().ToLowerInvariant();
                c.Remove(GetKey("interactive"));
                c.Remove(GetKey("passive"));
                c.Remove(GetKey("silent"));
            }

            return c;
        }
    }
}