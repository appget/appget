using System;

namespace AppGet
{
    public interface IEnvironmentProxy
    {
        OperatingSystem OSVersion { get; set; }
    }

    public class EnvironmentProxy : IEnvironmentProxy
    {
        public OperatingSystem OSVersion { get; set; }
    }
}
