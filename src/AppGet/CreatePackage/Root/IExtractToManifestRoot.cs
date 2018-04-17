using AppGet.CreatePackage.ManifestBuilder;

namespace AppGet.CreatePackage.Root
{
    public interface IExtractToManifestRoot
    {
        void Invoke(PackageManifestBuilder manifestBuilder);
    }

    public interface IManifestPrompt
    {
        bool ShouldPrompt(PackageManifestBuilder manifestBuilder);
        void Invoke(PackageManifestBuilder manifestBuilder);
    }
}