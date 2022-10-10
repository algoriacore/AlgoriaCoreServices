namespace AlgoriaCore.Application.QueriesAndCommands.Files._1Model
{
    public class GetFileResponse
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public byte[] FileArray { get; set; }
        public string ContentType { get; set; }
        public string DownloadFileName { get; set; }
    }
}
