using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using AppGet.Extensions;
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

        public string YamlText => _fileSystem.FileExists(FilePath) ? _fileSystem.ReadAllText(FilePath) : null;

        public T Load()
        {
            var yaml = YamlText;

            T data = null;

            if (!yaml.IsNullOrWhiteSpace())
            {
                data = Yaml.Deserialize<T>(YamlText);
            }

            if (data == null)
            {
                data = new T();
            }

            LoadDefaults(data);
            SetInitialValues(data);

            return data;
        }

        private void LoadDefaults(T data)
        {
            var props = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (!(prop.GetCustomAttributes().FirstOrDefault(c => c is DefaultValueAttribute) is DefaultValueAttribute defaultValueAttribute)) continue;

                if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                {
                    throw new InvalidOperationException($"Config Property {prop.Name} has a DefaultValue attribute but is not nullable");
                }

                if (prop.GetValue(data) == null)
                {
                    prop.SetValue(data, defaultValueAttribute.Value);
                }
            }
        }

        protected virtual void SetInitialValues(T data)
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