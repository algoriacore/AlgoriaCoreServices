namespace AlgoriaCore.Domain.Interfaces.Folder
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }
        string WebLogsFolder { get; set; }
        string TemplatesFolder { get; set; }
    }
}
