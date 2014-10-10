using System;

namespace AppGet.Requirements
{
    public class EnforcementResult
    {
        public static EnforcementResult Pass()
        {
            return new EnforcementResult
            {
                Success = true
            };
        }

        public static EnforcementResult Fail(string reason, params object[] args)
        {
            return Fail(String.Format(reason, args));
        }

        public static EnforcementResult Fail(string reason)
        {
            return new EnforcementResult
            {
                Success = false,
                Reason = reason
            };
        }

        public bool Success { get; private set; }
        public string Reason { get; private set; }

    }
}
