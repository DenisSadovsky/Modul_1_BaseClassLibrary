using System;
using System.Collections.Generic;
using System.IO;
using Modul_1_Base_Class_Library.Interfases;
using Modul_1_Base_Class_Library.EventArgs;
using Modul_1_Base_Class_Library.Models;

namespace Modul_1_Base_Class_Library
{
    class FilesWatcher : ILocationsWatcher<FileModel>
    {
        private IList<FileSystemWatcher> _fileSystemWatchers;
        private ILogger _logger;

        public FilesWatcher(ICollection<string> directories, ILogger logger)
        {
            _logger = logger;
            _fileSystemWatchers = new List<FileSystemWatcher>(directories.Count);

            foreach (var dir in directories)
            {
                _fileSystemWatchers.Add(CreateWatcher(dir));
            }
        }

        public event EventHandler<CreatedEventArgs<FileModel>> Created;

        private void OnCreated(FileModel file)
        {
            Created?.Invoke(this, new CreatedEventArgs<FileModel> { CreatedItem = file });
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            FileSystemWatcher fileSystemWatcher =
                new FileSystemWatcher(path)
                {
                    NotifyFilter = NotifyFilters.FileName,
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true
                };
            fileSystemWatcher.Created += (sender, fileSystemEventArgs) =>
            {
                _logger.Log($"File founded: {fileSystemEventArgs.Name}");
                OnCreated(new FileModel { FullName = fileSystemEventArgs.FullPath, Name = fileSystemEventArgs.Name });
            };

            return fileSystemWatcher;
        }
    }
}
