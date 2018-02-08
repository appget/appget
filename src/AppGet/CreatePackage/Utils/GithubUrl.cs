using System;

namespace AppGet.CreatePackage.Utils
{
    public class GithubUrl
    {
        private readonly Uri _uri;
        private readonly string[] _parts;

        public GithubUrl(string url)
        {
            _uri = new Uri(url, UriKind.Absolute);
            _parts = _uri.LocalPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public bool IsValid => _uri.Host == "github.com";

        public string OrganizationName
        {
            get
            {
                if (!IsValid) return null;
                if (_parts.Length < 1) return "";

                return _parts[0];
            }
        }

        public string RepositoryUrl
        {
            get
            {
                if (!IsValid) return null;
                if (_parts.Length < 2) return "";

                return $"https://github.com/{OrganizationName}/{_parts[1]}";
            }
        }
    }
}