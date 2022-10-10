namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitForListResponse
    {
        public long Id { get; set; }
        public long? ParentOU { get; set; }
        public string ParentOUDesc { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public bool HasChildren { get; set; }
        public int Size { get; set; }
    }
}