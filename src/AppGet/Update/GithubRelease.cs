using System.Collections.Generic;
using System.Runtime.Serialization;

// ReSharper disable once InconsistentNaming
namespace AppGet.Update
{
    public class GithubRelease
    {
        public string tag_name { get; set; }
        public List<GithubReleaseAsset> Assets { get; set; }
    }

    public class GithubReleaseAsset
    {
        public string browser_download_url { get; set; }
    }
}