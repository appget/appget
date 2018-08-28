using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AppGet.FileSystem
{
    public interface IFileSystem
    {
        void EnsureDirectory(string path);
        bool FileExists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
        void CreateDirectory(string path);
        void SetPermissions(string path, WellKnownSidType accountSid, FileSystemRights rights, AccessControlType controlType);
        void DeleteFile(string tempFile);
        void Move(string sourceFile, string destinationFile);

        Stream Open(string path, FileMode fileMode, FileAccess fileAccess);
    }

    public class FileSystem : IFileSystem
    {
        public void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void SetPermissions(string path, WellKnownSidType accountSid, FileSystemRights rights, AccessControlType controlType)
        {
            var sid = new SecurityIdentifier(accountSid, null);
            var directoryInfo = new DirectoryInfo(path);
            var directorySecurity = directoryInfo.GetAccessControl();

            var accessRule = new FileSystemAccessRule(sid, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None,
                controlType);

            directorySecurity.AddAccessRule(accessRule);
            directoryInfo.SetAccessControl(directorySecurity);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public void Move(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        public Stream Open(string path, FileMode fileMode, FileAccess fileAccess)
        {
            return File.Open(path, fileMode, fileAccess);
        }
    }
}