using AlgoriaCore.Domain.Attributes;
using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities.MongoDb
{
    [BsonCollectionName("CatalogsCustom")]
    [BsonIgnoreExtraElements]
    public partial class CatalogCustom: MongoDbEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string Description { get; set; }
        public bool IsCollectionGenerated { get; set; }
        public string NamePlural { get; set; }
        public string NameSingular { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Questionnarie { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public List<string> FieldNames { get; set; }
    }
}
