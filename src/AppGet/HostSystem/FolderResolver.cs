using System;
using System.IO;

namespace AppGet.HostSystem
{
    public class FolderResolver
    {
        private string TempFolder
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        private string ProgramData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

      
    }
}