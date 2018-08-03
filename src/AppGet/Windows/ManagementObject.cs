using System;
using System.Collections.Generic;
using System.Management;

namespace AppGet.Windows
{
    public class ManagementObject
    {
        public IEnumerable<int> QueryProcess(string query)
        {
            var sql = $"SELECT ProcessId FROM Win32_Process WHERE {query}";
            var searcher = new ManagementObjectSearcher(sql);

            foreach (var item in searcher.Get())
            {
                yield return Convert.ToInt32(item["processId"]);
            }
        }

        public IEnumerable<int> GetProcessByPath(string path)
        {
            path = path.TrimEnd('\\') + "\\";
            path = path.Replace("\\", "\\\\");

            return QueryProcess($"ExecutablePath LIKE '{path}%'");
        }
    }
}
