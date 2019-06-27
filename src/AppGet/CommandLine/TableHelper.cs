using System;
using System.Collections.Generic;
using AppGet.Manifest;
using AppGet.Update;

namespace AppGet.CommandLine
{
    public static class TableHelper
    {
        public static void ShowTable(this IEnumerable<PackageManifest> packages)
        {
            var table = new ConsoleTable("ID", "Name", "Version", "Tag");

            foreach (var package in packages)
            {
                table.AddRow(package.Id, package.Name, package.Version, package.Tag);
            }

            Print(table);
        }

        public static void ShowTable(this IEnumerable<PackageUpdate> updates, bool printStatus = true)
        {
            var columns = new List<string>
            {
                "Name", "Package ID", "Installed Version", "Available Version"
            };

            if (printStatus)
            {
                columns.Add("Status");
            }

            var table = new ConsoleTable(columns.ToArray());

            foreach (var update in updates)
            {
                var cols = new List<object>
                {
                    update.Name, update.PackageId, update.InstalledVersion, update.AvailableVersion
                };

                if (printStatus)
                {
                    cols.Add(update.Status);
                }

                table.AddRow(cols.ToArray());
            }

            Print(table);
        }

        public static void Print(this ConsoleTable table)
        {
            Console.WriteLine();
            table.Write(Format.MarkDown);
            Console.WriteLine();
        }
    }
}