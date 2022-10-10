using AlgoriaCore.Domain.Attributes;
using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities.MongoDb
{
    [BsonCollectionName("Questionnaires")]
    [BsonIgnoreExtraElements]
    public partial class Questionnaire: MongoDbEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string Name { get; set; }
        public string CustomCode { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public List<Section> Sections { get; set; }

        public Questionnaire()
        {
            Sections = new List<Section>();
        }
    }
}
