using System;
using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Manifest.Serialization;

namespace AppGet.AppData
{
    public interface IStore<T> where T : new()
    {
        T Load();
        void Save(T data);
        void Save(Action<T> set);
    }

    public abstract class StoreBase<T> : IStore<T> where T : class, new()
    {
        private readonly IFileSystem _fileSystem;
        private readonly IPathResolver _pathResolver;

        protected abstract string Name { get; }

        protected string FilePath => Path.Combine(_pathResolver.AppDataDirectory, $"{Name}.yaml");

        protected StoreBase(IFileSystem fileSystem, IPathResolver pathResolver)
        {
            _fileSystem = fileSystem;
            _pathResolver = pathResolver;
        }

        public T Load()
        {
            T data = null;
            if (_fileSystem.FileExists(FilePath))
            {
                var content = _fileSystem.ReadAllText(FilePath);
                data = Yaml.Deserialize<T>(content);
            }

            if (data == null)
            {
                data = new T();
            }

            SetDefaults(data);

            return data;
        }

        protected virtual void SetDefaults(T data)
        {

        }

        public void Save(T data)
        {
            _fileSystem.EnsureDirectory(_pathResolver.AppDataDirectory);
            var yaml = Yaml.Serialize(data);
            _fileSystem.WriteAllText(FilePath, yaml);
        }

        public void Save(Action<T> set)
        {
            _fileSystem.EnsureDirectory(_pathResolver.AppDataDirectory);
            var data = Load();
            set(data);
            var yaml = Yaml.Serialize(data);
            _fileSystem.WriteAllText(FilePath, yaml);
        }
    }
}