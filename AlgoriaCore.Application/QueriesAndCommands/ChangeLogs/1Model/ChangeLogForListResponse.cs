using System;

namespace AlgoriaCore.Application.QueriesAndCommands.ChangeLogs
{
    public class ChangeLogForListResponse
    {
        public long? Id { get; set; }
        public long? User { get; set; }
        public string UserDesc { get; set; }
        public DateTime? Datetime { get; set; }
        public string Detail { get; set; }
    }
}