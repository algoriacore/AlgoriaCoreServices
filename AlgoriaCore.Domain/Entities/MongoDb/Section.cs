using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities.MongoDb
{
    public partial class Section
    {
        public string IconAF { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<Field> Fields { get; set; }

    }
}
