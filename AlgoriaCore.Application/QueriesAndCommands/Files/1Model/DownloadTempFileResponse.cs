namespace AlgoriaCore.Application.QueriesAndCommands.Files._1Model
{
    public class DownloadTempFileResponse
    {
        public string FileName { get; set; }
        public int Length { get; set; }
        public byte[] FileArray { get; set; }
        public string ContentType { get; set; }
    }
}
