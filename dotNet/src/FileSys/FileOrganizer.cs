using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSys
{
    public class FileOrganizer
    {
        private DirectoryInfo _directory;

        public FileOrganizer For(DirectoryInfo directory)
        {
            this._directory = directory;
            return this;
        }
        
        public void GroupByCreationDate(string targetRootDirectoryPath = null, string directoryNameFormat = "MMM dd, yy", params string[] fileFilters)
        {
            fileFilters = fileFilters ?? new[] {"*.*"};

            var createdAtToFileMappings = fileFilters.SelectMany(f => this._directory.GetFiles(f, SearchOption.AllDirectories))
                .ToLookup(fileInfo => fileInfo.CreationTime.Date.ToString(directoryNameFormat));

            GroupFilesInDirectory(targetRootDirectoryPath, createdAtToFileMappings);
        }

        readonly string[] _imageFilters = { "*.jpg", "*.png", "*.gif" };

        public void GroupImagesByDateTaken(string targetRootDirectoryPath = null, string directoryNameFormat = "MMM dd, yy")
        {
            var createdAtToFileMappings = _imageFilters.SelectMany(f => this._directory.GetFiles(f, SearchOption.AllDirectories))
                .ToLookup(fileInfo => fileInfo.GetDateTakenFromImage().ToString(directoryNameFormat));

            GroupFilesInDirectory(targetRootDirectoryPath, createdAtToFileMappings);
        }

        private void GroupFilesInDirectory(string targetRootDirectoryPath, ILookup<string, FileInfo> parentDirectoryToFileMappings)
        {
            var targetRootDirectory = targetRootDirectoryPath != null
                ? new DirectoryInfo(targetRootDirectoryPath)
                : this._directory;

            targetRootDirectory.EnsureExist();

            foreach (var parentToFileMapping in parentDirectoryToFileMappings)
            {
                var targetDirectory = targetRootDirectory.CreateSubdirectory(parentToFileMapping.Key);

                foreach (var fileInfo in parentToFileMapping)
                {
                    fileInfo.MoveTo(Path.Combine(targetDirectory.FullName, fileInfo.Name));
                }
            }
        }
    }

    public static class FileOrganizerExt
    {
        public static FileOrganizer Organizer(this DirectoryInfo directoryInfo)
        {
            return new FileOrganizer().For(directoryInfo);
        }
    }
}
