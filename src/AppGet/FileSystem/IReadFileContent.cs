using System.IO;

namespace AppGet.FileSystem
{
    public interface IFileSystem
    {
        string ReadAllText(string source);
        bool FileExists(string source);
        void WriteAllText(string path, string content);
    }


    public class FileSystem : IFileSystem
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}