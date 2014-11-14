using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppGet.Processes
{
    public static class FileInUse
    {
        private const int RmRebootReasonNone = 0;
        private const int SUCCESS = 0;
        private const int CCH_RM_MAX_APP_NAME = 255;

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern int RmRegisterResources(uint pSessionHandle,
            UInt32 nFiles,
            string[] rgsFilenames,
            UInt32 nApplications,
            [In] RM_UNIQUE_PROCESS[] rgApplications,
            UInt32 nServices,
            string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        private static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmGetList(uint dwSessionHandle,
            out uint pnProcInfoNeeded,
            ref uint pnProcInfo,
            [In, Out] RmProcessInfo[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        //http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        //http://wyupdate.googlecode.com/svn-history/r401/trunk/frmFilesInUse.cs (New BSD License)
        public static IEnumerable<Process> GetLockers(params string[] files)
        {
            uint handle;
            var key = Guid.NewGuid().ToString();
            var processes = new List<Process>();

            var startSessionCode = RmStartSession(out handle, 0, key);
            if (startSessionCode != SUCCESS)
            {
                throw new Exception("Could not begin restart session. Unable to determine file locker. Error:" + startSessionCode);
            }

            try
            {
                const int ERROR_MORE_DATA = 234;

                string[] resources = files; // Just checking on one resource.

                var registerResourcesCode = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (registerResourcesCode != SUCCESS)
                {
                    throw new Exception("Could not register resource. Error:" + registerResourcesCode);
                }

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                uint pnProcInfo = 0;
                uint pnProcInfoNeeded;
                uint lpdwRebootReasons = RmRebootReasonNone;

                var getListCode = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (getListCode == ERROR_MORE_DATA)
                {
                    return GetProcessList(pnProcInfoNeeded, handle, lpdwRebootReasons);
                }
                else if (getListCode != SUCCESS)
                {
                    throw new Exception("Could not list processes locking resource. Code:" + getListCode);
                }

            }
            finally
            {
                RmEndSession(handle);
            }

            return processes;
        }

        private static List<Process> GetProcessList(uint pnProcInfoNeeded, uint handle, uint lpdwRebootReasons)
        {
            var processList = new List<Process>();
            var processInfo = new RmProcessInfo[pnProcInfoNeeded];
            uint pnProcInfo = pnProcInfoNeeded;

            var getListCode = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
            if (getListCode != SUCCESS)
            {
                throw new Exception("Could not list processes locking resource. Error:" + getListCode);
            }

            for (var i = 0; i < pnProcInfo; i++)
            {
                try
                {
                    var processId = processInfo[i].Process.dwProcessId;
                    var process = Process.GetProcessById(processId);
                    processList.Add(process);
                }
                catch (AccessViolationException)
                {
                }
                catch (ArgumentException)
                {
                }
            }

            return processList;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct RmProcessInfo
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public readonly string strAppName;

        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RM_UNIQUE_PROCESS
        {
            public readonly int dwProcessId;
        }
    }
}