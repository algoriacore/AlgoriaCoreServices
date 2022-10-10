using System;

namespace AlgoriaCore.Application.Managers.ChangeLogs.Dto
{
    public class ChangeLogDto
    {
        public long? Id { get; set; }
        public long? User { get; set; }
        public string UserDesc { get; set; }
        public string Table { get; set; }
        public string Key { get; set; }
        public DateTime? Datetime { get; set; }
        public string Detail { get; set; }
    }

    public enum ChangeLogType
    {
        Delete = 0,
        Create = 1,
        Update = 2,
        SoftDelete = 3
    }
}
