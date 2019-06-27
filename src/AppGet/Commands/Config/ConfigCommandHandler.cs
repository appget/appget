using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppGet.AppData;
using AppGet.CommandLine;
using AppGet.Commands.AddRepo;
using AppGet.Infrastructure.Composition;
using AppGet.Manifest.Serialization;

namespace AppGet.Commands.Config
{
    [Handles(typeof(ConfigOptions))]
    public class ConfigCommandHandler : ICommandHandler
    {
        private readonly ConfigStore _configStore;

        public ConfigCommandHandler(ConfigStore configStore)
        {
            _configStore = configStore;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var configOptions = (ConfigOptions)commandOptions;

            var action = configOptions.Action.ToLower().Trim();

            switch (action)
            {
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
            var props = config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var table = new ConsoleTable("Key", "Value", "Type");
            foreach (var prop in props)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType);
                if (type == null) type = prop.PropertyType;

                table.Rows.Add(new []
                {
                    prop.Name,
                    prop.GetValue(config),
                    type.Name
                });
            }
            
            table.Print();
        }
    }
}