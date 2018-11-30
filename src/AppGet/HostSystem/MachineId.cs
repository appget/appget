using System;
using AppGet.Manifest.Hash;
using Microsoft.Win32;
using NLog;

namespace AppGet.HostSystem
{
    public class MachineId
    {
        private readonly Logger _logger;
        private readonly ICalculateHash _hash;

        public MachineId(Logger logger, ICalculateHash hash)
        {
            _logger = logger;
            _hash = hash;
        }

        private string ReadMachineKey()
        {
            try
            {
                using (var guid = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    var machineGuid = Guid.ParseExact(guid.GetValue("MachineGuid").ToString(), "d");

                    return _hash.CalculateHash(machineGuid.ToByteArray());
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Couldn't read Machine GUID.");
                return new string('=', 10);
            }
        }

        public Lazy<string> MachineKey => new Lazy<string>(ReadMachineKey);
    }
}