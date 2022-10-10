namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserForListResponse
    {
        public long Id { get; set; }
        public long OrgUnit { get; set; }
        public string OrgUnitDesc { get; set; }
        public long User { get; set; }
        public string UserDesc { get; set; }
    }
}