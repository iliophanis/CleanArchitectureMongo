using System;
using Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Persistence.ClassMaps
{
    public class AuditableEntityClassMap : BsonClassMap<AuditableEntity>
    {
        public AuditableEntityClassMap()
        {
            AutoMap();
            SetIdMember(GetMemberMap(m => m.Id)
               .SetSerializer(new StringSerializer(BsonType.ObjectId))
               .SetIdGenerator(StringObjectIdGenerator.Instance));
            MapMember(m => m.DateCreated)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc, BsonType.DateTime));
            MapMember(m => m.DateLastUpdated)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc, BsonType.DateTime));
        }
    }
}