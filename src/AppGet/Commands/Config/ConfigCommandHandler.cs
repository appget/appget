using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppGet.AppData;
using AppGet.CommandLine;
using AppGet.Extensions;
using AppGet.Infrastructure.Composition;

namespace AppGet.Commands.Config
{
    [Handles(typeof(ConfigOptions))]
    public class ConfigCommandHandler : ICommandHandler
    {
        private readonly ConfigStore _configStore;
        private readonly PropertyInfo[] _configProps;

        public ConfigCommandHandler(ConfigStore configStore)
        {
            _configStore = configStore;
            _configProps = typeof(AppData.Config).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var configOptions = (ConfigOptions)commandOptions;

            var action = configOptions.Action.ToLower().Trim();

            switch (action)
            {
                case "set":
                {
                    if (configOptions.Key.IsNullOrWhiteSpace())
                    {
                        throw new InvalidCommandParamaterException("Parameter 'Key' is required", commandOptions);
                    }

                    var key = configOptions.Key.Trim();

                    var prop = _configProps.SingleOrDefault(c => string.Equals(c.Name, key, StringComparison.CurrentCultureIgnoreCase));

                    if (prop == null)
                    {
                        throw new InvalidCommandParamaterException($"'{configOptions.Key}' is not a valid config key.", commandOptions);
                    }

                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);
                    var convertedValue = converter.ConvertFromInvariantString(configOptions.Value);
                    _configStore.Save(config => prop.SetValue(config, convertedValue));
                    break;
                }

                case "list":
                case "view":
                {
                    var config = _configStore.Load();
                    PrintConfig(config);

                    break;
                }
            }
        }

        private void PrintConfig(AppData.Config config)
        {
            var table = new ConsoleTable("Key", "Value", "Type");
            foreach (var prop in _configProps)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType);
                if (type == null) type = prop.PropertyType;

                table.Rows.Add(new[]
                {
                    prop.Name, prop.GetValue(config), type.Name
                });
            }

            table.Print();
        }
    }
}