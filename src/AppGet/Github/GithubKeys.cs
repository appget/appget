namespace AppGet.Github
{
    public static class GithubKeys
    {
        private const string CLIENT_ID = "5c03b44d2169c10ec7ab";
        private const string CLIENT_SECRET = "7f550304d00757d6e905ae0647d041fd9e3cbf7c";

        public static readonly string AuthQuery = $"client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}";
    }
}