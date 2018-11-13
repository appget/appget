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
    }
}