using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlgoriaCore.Domain.Entities.MongoDb
{
    public partial class CatalogCustomRelation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string idCatalogCustom { get; set; }
        public string FieldName { get; set; }
    }
}
