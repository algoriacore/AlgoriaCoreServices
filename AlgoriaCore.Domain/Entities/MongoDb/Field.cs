using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities.MongoDb
{
    public partial class Field
    {
        public int FieldControl { get; set; }
        public string FieldName { get; set; }
        public int? FieldSize { get; set; }
        public int FieldType { get; set; }
        public bool HasKeyFilter { get; set; }
        public string InputMask { get; set; }
        public bool IsRequired { get; set; }
        public string KeyFilter { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<FieldOption> Options { get; set; }
        public CatalogCustomRelation CatalogCustom { get; set; }
        public CustomProperties CustomProperties { get; set; }
    }
}
