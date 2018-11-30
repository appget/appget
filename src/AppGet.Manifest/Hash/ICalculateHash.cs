using System;

namespace AppGet.Manifest.Hash
{
    public interface ICalculateHash
    {
        string CalculateHash(string file);
        string CalculateHash(byte[] buffer);
    }
}