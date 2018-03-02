namespace AppGet.CreatePackage.Installer
{
    public interface IInstallerPrompt
    {
        bool ShouldPrompt(InstallerBuilder installerBuilder);
        void Invoke(InstallerBuilder installerBuilder);
    }
}