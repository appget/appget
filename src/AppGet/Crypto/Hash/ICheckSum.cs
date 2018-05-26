using AppGet.Manifest;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash
{
    public interface ICheckSum
    {
        HashTypes HashType { get; }
        string CalculateHash(string file);
    }
}