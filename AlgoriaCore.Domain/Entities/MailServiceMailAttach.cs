using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceMailAttach : Entity<long>
    {
        public long? MailServiceMailBody { get; set; }
        public string ContenType { get; set; }
        public string FileName { get; set; }
        public byte[] BinaryFile { get; set; }

        public virtual MailServiceMailBody MailServiceMailBodyNavigation { get; set; }
    }
}
