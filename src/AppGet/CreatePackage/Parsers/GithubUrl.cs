using System;

namespace AppGet.CreatePackage.Parsers
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

        public string Owner
        {
            get
            {
                if (!IsValid) return null;
                if (_parts.Length < 1) return "";

                return _parts[0];
            }
        }

        public string Repository
        {
            get
            {
                if (!IsValid) return null;
                if (_parts.Length < 2) return "";

                return _parts[1];
            }
        }

        public string RepositoryUrl
        {
            get
            {
                if (!IsValid) return null;
                if (_parts.Length < 2) return "";

                return $"https://github.com/{Owner}/{_parts[1]}";
            }
        }
    }
}