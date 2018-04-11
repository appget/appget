namespace AppGet.Github
{
    public static class GithubKeys
    {
        private const string CLIENT_ID = "cf0e72e91b6b455431bf";
        private const string CLIENT_SECRET = "d8056adeed4b4a6e0f73caa71f29e71ab2e3dbbf";

        public static readonly string AuthQuery = $"client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}";
    }
}