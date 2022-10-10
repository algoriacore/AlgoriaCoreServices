using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlgoriaCore.Domain.Entities.Base
{
    public abstract class MongoDbEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
