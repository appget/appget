namespace AppGet.AppData
{
    public interface IConfig
    {
        string LocalRepository { get; }
    }

    public class Config : IConfig
    {
        public string LocalRepository => @"C:\git\AppGet.Packages\manifests\";
    }
}
