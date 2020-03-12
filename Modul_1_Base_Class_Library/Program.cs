using System;
using System.Threading;
using System.Threading.Tasks;
using Modul_1_Base_Class_Library.Interfases;
using Modul_1_Base_Class_Library.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using Modul_1_Base_Class_Library.EventArgs;
using Modul_1_Base_Class_Library.Resources;
using DirectoryElement = Modul_1_Base_Class_Library.Configuration.DirectoryElement;
using FileSystemMonitorConfigSection = Modul_1_Base_Class_Library.Configuration.FileSystemMonitorConfigSection;
using RuleElement = Modul_1_Base_Class_Library.Configuration.RuleElement;

namespace Modul_1_Base_Class_Library
{
    class Program
    {
       
        private static List<string> _directories;
        private static List<Rule> _rules;
        private static IDistributor<FileModel> _distributor;
        static async Task Main(string[] args)
        {
            FileSystemMonitorConfigSection config = ConfigurationManager.GetSection("fileSystemSection") as FileSystemMonitorConfigSection;

            if (config != null)
            {
                ReadConfiguration(config);
            }
            else
            {
                Console.Write(Strings.ConfigNotFounded);
                return;
            }

            Console.WriteLine(config.Culture.DisplayName);

            ILogger logger = new Logger();
            _distributor = new FilesDistributor(_rules, config.Rules.DefaultDirectory, logger);
            ILocationsWatcher<FileModel> watcher = new FilesWatcher(_directories, logger);

            watcher.Created += OnCreated;

            CancellationTokenSource source = new CancellationTokenSource();

            Console.CancelKeyPress += (o, e) =>
            {
                watcher.Created -= OnCreated;
                source.Cancel();
            };

            await Task.Delay(TimeSpan.FromMilliseconds(-1), source.Token);
        }

        private static void ReadConfiguration(FileSystemMonitorConfigSection config)
        {
            _directories = new List<string>(config.Directories.Count);
            _rules = new List<Rule>();

            foreach (DirectoryElement directory in config.Directories)
            {
                _directories.Add(directory.Path);
            }

            foreach (RuleElement rule in config.Rules)
            {
                _rules.Add(new Rule
                {
                    FilePattern = rule.FilePattern,
                    DestinationFolder = rule.DestinationFolder,
                    IsDateAppended = rule.IsDateAppended,
                    IsOrderAppended = rule.IsOrderAppended
                });
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
            CultureInfo.CurrentUICulture = config.Culture;
            CultureInfo.CurrentCulture = config.Culture;
        }

        private static async void OnCreated(object sender, CreatedEventArgs<FileModel> args)
        {
            await _distributor.MoveAsync(args.CreatedItem);
        }







        //private static void Run()
        //{
        //    Console.WriteLine("My Watcher");

        //    string gotor = "C:\\New folder";

        //    FileSystemWatcher watcher = new FileSystemWatcher();
        //    watcher.Path = gotor;
        //    watcher.NotifyFilter = NotifyFilters.LastAccess
        //                            | NotifyFilters.LastWrite
        //                            | NotifyFilters.FileName
        //                            | NotifyFilters.DirectoryName;

        //    watcher.Filter = "*.txt";


        //    watcher.Changed += OnChanged;
        //    watcher.Created += OnCreated;
        //    watcher.Deleted += OnDeleted;
        //    watcher.Renamed += OnRenamed;

        //    // Begin watching.
        //    watcher.EnableRaisingEvents = true;

        //    // Wait for the user to quit the program.
        //    Console.WriteLine("Press 'q' to quit the sample.");
        //    while (Console.Read() != 'q') ;
        //}


        //// Define the event handlers.
        //private static void OnChanged(object source, FileSystemEventArgs e) =>
        //    // Specify what is done when a file is changed, created, or deleted.
        //    Console.WriteLine($"File: {e.FullPath} @Action: {e.ChangeType}");
        //private static void OnCreated(object source, FileSystemEventArgs e) =>
        //    // Specify what is done when a file is changed, created, or deleted.
        //    Console.WriteLine($"File: {e.FullPath} @Action: {e.ChangeType}");
        //private static void OnDeleted(object source, FileSystemEventArgs e) =>
        //    // Specify what is done when a file is changed, created, or deleted.
        //    Console.WriteLine($"File: {e.FullPath} @Action: {e.ChangeType}");

        //private static void OnRenamed(object source, RenamedEventArgs e) =>
        //    // Specify what is done when a file is renamed.
        //    Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");


    }
}
