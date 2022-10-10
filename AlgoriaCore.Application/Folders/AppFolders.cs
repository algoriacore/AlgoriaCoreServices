using AlgoriaCore.Domain.Interfaces.Folder;

namespace AlgoriaCore.Application.Folders
{
    public class AppFolders : IAppFolders
    {
        public string TempFileDownloadFolder { get; set; }

        public string WebLogsFolder { get; set; }

        public string TemplatesFolder { get; set; }
    }
}
