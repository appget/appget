using AppGet.Manifests;

namespace AppGet.Crypto.Hash
{
    public interface ICheckSum
    {
        HashType HashType { get; }
        string CalculateHash(string file);
    }
}