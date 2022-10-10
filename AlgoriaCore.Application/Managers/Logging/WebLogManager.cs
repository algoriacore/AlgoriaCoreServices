using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Folders;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Logging.Dto;
using AlgoriaCore.Domain.Interfaces.Folder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgoriaCore.Application.Managers.Logging
{
    public class WebLogManager : BaseManager
    {
        private readonly IAppFolders _appFolders;

        public WebLogManager(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        public WebLogLastestDto GetLatestWebLogs()
        {
            var directory = new DirectoryInfo(_appFolders.WebLogsFolder);
            if (!directory.Exists)
            {
                return new WebLogLastestDto { LatesWebLogLines = new List<string>() };
            }

            var lastLogFile = directory.GetFiles("*.txt", SearchOption.AllDirectories)
                                        .OrderByDescending(f => f.LastWriteTime)
                                        .FirstOrDefault();

            if (lastLogFile == null)
            {
                return new WebLogLastestDto();
            }

            var lines = AppFileHelper.ReadLines(lastLogFile.FullName).Reverse().Take(1000).ToList();
            var logLineCount = 0;
            var lineCount = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("DEBUG") ||
                    line.StartsWith("INFO") ||
                    line.StartsWith("WARN") ||
                    line.StartsWith("ERROR") ||
                    line.StartsWith("FATAL"))
                {
                    logLineCount++;
                }

                lineCount++;

                if (logLineCount == 100)
                {
                    break;
                }
            }

            return new WebLogLastestDto
            {
                LatesWebLogLines = lines.Take(lineCount).Reverse().ToList()
            };
        }

        public FileDto DownloadWebLogs()
        {
            //Crear copia temporal de logs
            var tempLogDirectory = CopyAllLogFilesToTempDirectory();
            var logFiles = new DirectoryInfo(tempLogDirectory).GetFiles("*.txt", SearchOption.TopDirectoryOnly).ToList();

            //Crear el archivo zip
            var zipFileDto = new FileDto("WebSiteLogs.zip", MimeTypeNames.ApplicationZip);
            var outputZipFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, zipFileDto.FileToken);

            using (var outputZipFileStream = File.Create(outputZipFilePath))
            {
                using (var zipStream = new System.IO.Compression.ZipArchive(outputZipFileStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var logFile in logFiles)
                    {
                        var logZipEntry = zipStream.CreateEntry(logFile.Name, System.IO.Compression.CompressionLevel.Fastest);
                        logZipEntry.LastWriteTime = logFile.LastWriteTime;

                        using (var zipEntry = logZipEntry.Open())
                        {
                            using (var fs = new FileStream(logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                            {
                                fs.CopyTo(zipEntry);
                            }
                        }
                    }
                }
            }

            //Eliminar copia temporal de logs
            Directory.Delete(tempLogDirectory, true);

            return zipFileDto;
        }

        private string CopyAllLogFilesToTempDirectory()
        {
            var tempDirectoryPath = Path.Combine(_appFolders.TempFileDownloadFolder, Guid.NewGuid().ToString("N").Substring(16));
            DirectoryHelper.CreateIfNotExists(tempDirectoryPath);

            foreach (var file in GetAllLogFiles())
            {
                var destinationFilePath = Path.Combine(tempDirectoryPath, file.Name);
                File.Copy(file.FullName, destinationFilePath, true);
            }

            return tempDirectoryPath;
        }

        private List<FileInfo> GetAllLogFiles()
        {
            var directory = new DirectoryInfo(_appFolders.WebLogsFolder);
            return directory.GetFiles("*.txt", SearchOption.TopDirectoryOnly).ToList();
        }

    }
}
