using System;

namespace AppGet.Http
{
    public class AppGetApiError
    {
        public string Message { get; set; }
        public Version Ver { get; set; }
        public string Tracking { get; set; }
        public string Host { get; set; }

        public override string ToString()
        {
            return $"{Message}; code: {Tracking}";
        }
    }
}