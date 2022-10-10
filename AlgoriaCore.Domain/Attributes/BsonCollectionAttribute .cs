using System;

namespace AlgoriaCore.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionNameAttribute : Attribute
    {
        public BsonCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName
        {
            get;
        }
    }
}
