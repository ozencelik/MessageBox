using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MessageBox.Data.BaseEntities
{
    public abstract class BaseMongoEntity : BaseEntity
    {
        /// <summary>
        /// Used as primary key for all entities
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
    }
}
