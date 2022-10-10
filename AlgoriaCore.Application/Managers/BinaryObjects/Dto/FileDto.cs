namespace AlgoriaCore.Application.Managers.BinaryObjects.Dto
{
    public class FileDto
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int Length { get; set; }
        public byte[] FileArray { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
    }
}
