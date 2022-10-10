namespace AlgoriaCore.Domain.Interfaces.Email
{
    public interface IEmailAttachment
    {
        string FileName { get; set; }
        string ContentType { get; set; }
        byte[] FileArray { get; set; }
    }
}
