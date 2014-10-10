namespace AppGet.Requirements
{
    public class EnforcementResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public EnforcementResult(bool success)
        {
            Success = success;
        }

        public EnforcementResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
