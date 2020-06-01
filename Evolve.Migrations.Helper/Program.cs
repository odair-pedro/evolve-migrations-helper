using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Evolve.Migrations.Helper
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Contains("--version"))
            {
                PrintHeaderMessage();
                return;
            }
            
            if (!ValidateArguments(args))
            {
                return;
            }

            var rootPath = GetRootPath(args);
            if (rootPath == default)
            {
                return;
            }

            var fileName = GetFileName(args);
            if (fileName == default)
            {
                return;
            }

            var fileSeparator = GetFileSeparator(args);
            if (fileSeparator == default)
            {
                return;
            }

            var filePath = CreateFile(rootPath, fileName, fileSeparator);
            
            ChangeCsProjFile(rootPath, filePath);
        }

        private static bool ValidateArguments(IReadOnlyList<string> args)
        {
            if (args.Count == 0 || args[0] == "--help")
            {
                PrintHeaderMessage();
                Console.WriteLine($"\r\nUsage: {Assembly.GetExecutingAssembly().GetName().Name} [command] [options]\r\n");
                Console.WriteLine("Commands:");
                Console.WriteLine($"    {"add-dataset".PadRight(20, ' ')} Add a new migration file (on path: \"./datasets\")");
                Console.WriteLine($"    {"add-migration".PadRight(20, ' ')} Add a new migration file (on path: \"./migrations\")\r\n");
                Console.WriteLine("Options:");
                Console.WriteLine($"    {"-s|--separator".PadRight(20, ' ')} The file name seperator. Default is '__' (Double underscore). Eg: 'v{DateTime.Now:yyyyMMddHHmmss}__MyMigration.sql'");
                return false;
            }

            if (args.Count >= 2)
            {
                return true;
            }
            
            Console.WriteLine("Invalid command.");           
            PrintHelpMessage();
            return false;
        }

        private static string GetRootPath(IReadOnlyList<string> args)
        {
            var command = args[0];
            string rootPath;
            switch (command)
            {
                case "add-dataset":
                    rootPath = "db/datasets";
                    break;
                case "add-migration":
                    rootPath = "db/migrations";
                    break;
                default:
                    Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name}: '{command}' is not a valid command.\r\n");           
                    PrintHelpMessage();
                    return default;
            }

            return rootPath;
        }

        private static string GetFileName(IReadOnlyList<string> args)
        {
            return args[1];
        }

        private static string GetFileSeparator(IReadOnlyList<string> args)
        {
            const string dfSeparator = "__";
            if (args.Count < 3)
            {
                return dfSeparator;
            }

            try
            {
                return args[2] == "-s" || args[2] == "-separator" ? args[3] : throw new Exception();
            }
            catch
            {
                Console.WriteLine("Invalid options.");           
                PrintHelpMessage();
                return default;
            }
        }

        private static void PrintHeaderMessage()
        {
            Console.WriteLine("Evolve Migrations Helper");
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != default)
            {
                Console.WriteLine($"Version: {version.Major}.{version.Minor}.{version.Build}");
            }
        }

        private static void PrintHelpMessage()
        {
            Console.WriteLine($"Run '{Assembly.GetExecutingAssembly().GetName().Name} --help' for usage.");
        }

        private static string CreateFile(string rootPath, string fileName, string fileSeparator)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? "./", rootPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var filePath = Path.Combine(dir, $"v{DateTime.Now:yyyyMMddHHmmss}{fileSeparator}{fileName}.sql");
            using (var file = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                file.Flush();
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The file '{filePath}' has created sucessfully.");
            Console.ResetColor();
            return filePath;
        }

        private static void ChangeCsProjFile(string rootPath, string filePath)
        {
            var projPath = Path.GetDirectoryName(filePath.Replace(rootPath, string.Empty));
            var projFile = Directory.GetFiles(projPath, "*.csproj").FirstOrDefault();
            if (projFile == default)
            {
                return;
            }

            var xml = new XmlDocument();
            xml.Load(projFile);

            var nodeProject = xml.SelectSingleNode("//Project");
            if (nodeProject.SelectNodes("//ItemGroup").Count == 0)
            {
                var itemNew = xml.CreateElement("ItemGroup");
                nodeProject.AppendChild(itemNew);
            }

            var nodeItens = nodeProject.SelectNodes("//ItemGroup");
            var nodeItem = nodeItens[^1];
            var nodeNone = xml.CreateElement("EmbeddedResource");
            nodeNone.SetAttribute("Update", Path.Combine(rootPath, Path.GetFileName(filePath)));
            var nodeCopy = xml.CreateElement("CopyToOutputDirectory");
            nodeCopy.InnerText = "Always";
            nodeNone.AppendChild(nodeCopy);
            nodeItem.AppendChild(nodeNone);
            xml.Save(projFile);
        }
    }
}