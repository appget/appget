namespace AppGet.Installers
{
    public class ExistReason
    {
        public const string ERROR = "An error occurred during installation";
        public const string CANCELED = "Installation was canceled by the user";
        public const string RESTART_REQUIRED_FAILED = "A system restart is required before setup can continue";
        public const string RESTART_REQUIRED_SUCCESS = "A system restart is required to complete the installation";

        public ExistReason(ExitCodeTypes category, string message = null, bool success = false)
        {
            Category = category;
            Success = success;
            Message = message;

            if (message == null)
            {
                switch (category)
                {
                    case ExitCodeTypes.Failed:
                        {
                            Message = ERROR;

                            break;
                        }
                    case ExitCodeTypes.UserCanceled:
                        {
                            Message = CANCELED;

                            break;
                        }
                    case ExitCodeTypes.RestartRequired:
                        {
                            Message = success ? RESTART_REQUIRED_SUCCESS : RESTART_REQUIRED_FAILED;

                            break;
                        }
                    case ExitCodeTypes.RequirementUnmet:
                        {
                            Message = ERROR;

                            break;
                        }
                }
            }
        }

        public bool Success { get; }
        public string Message { get; }
        public ExitCodeTypes Category { get; }
    }
}