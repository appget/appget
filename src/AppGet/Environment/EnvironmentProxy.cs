using System;

namespace AppGet.Environment
{
    public interface IEnvironmentProxy
    {
        OperatingSystem OSVersion { get; }
        bool Is64BitOperatingSystem { get; }
    }

    public class EnvironmentProxy : IEnvironmentProxy
    {
        public OperatingSystem OSVersion { get { return System.Environment.OSVersion; } }
        public bool Is64BitOperatingSystem { get { return OsBitness.IsOs64Bit(); } }
    }
}
