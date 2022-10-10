namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model
{
    public class SessionLoginResponse
    {
        public int? TenantId { get; set; }
        public string TenancyName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } //Login
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string EMail { get; set; }
        public bool IsImpersonalized { get; set; }
        public long? ImpersonalizerUserId { get; set; }
    }
}