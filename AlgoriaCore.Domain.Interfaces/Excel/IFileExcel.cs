namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IFileExcel
    {
        string FileName { get; set; }

        string FileType { get; set; }

        string FileToken { get; set; }

        byte[] FileArray { get; set; }
    }
}
