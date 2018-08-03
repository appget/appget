using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AppGet.Windows
{
    public static class FileLockFinder
    {
        [StructLayout(LayoutKind.Sequential)]
        struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        const int RmRebootReasonNone = 0;
        const int CCH_RM_MAX_APP_NAME = 255;
        const int CCH_RM_MAX_SVC_NAME = 63;

        enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)] public bool bRestartable;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmRegisterResources(uint pSessionHandle, UInt32 nFiles, string[] rgsFilenames, UInt32 nApplications,
            [In] RM_UNIQUE_PROCESS[] rgApplications, UInt32 nServices, string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);


        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        public static IEnumerable<int> WhoIsLocking(params string[] resources)
        {
            const int ERROR_MORE_DATA = 234;
            const int SUCCESS = 0;


            if (RmStartSession(out var handle, 0, Guid.NewGuid().ToString()) != SUCCESS)
            {
                throw new Exception("Could not begin restart session.  Unable to determine file locker.");
            }

            try
            {
                uint pnProcInfo = 0;
                uint lpdwRebootReasons = RmRebootReasonNone;

                if (RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null) != SUCCESS)
                {
                    throw new Exception("Could not register resource.");
                }

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                var res = RmGetList(handle, out var pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == ERROR_MORE_DATA)
                {
                    var processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    // Get the list
                    if (RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons) != SUCCESS)
                    {
                        throw new Exception("Could not list processes locking resource.");
                    }

                    foreach (var p in processInfo)
                    {
                        yield return p.Process.dwProcessId;
                    }
                }
                else if (res != SUCCESS)
                {
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
                }
            }
            finally
            {
                RmEndSession(handle);
            }
        }
    }
}