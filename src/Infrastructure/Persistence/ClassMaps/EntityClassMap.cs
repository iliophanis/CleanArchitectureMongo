using Domain.Entities;
using MongoDB.Bson.Serialization;

namespace Infrastructure.Persistence.ClassMaps
{
    public class EntityClassMap : BsonClassMap<Entity>
    {
        public EntityClassMap()
        {
            AutoMap();
        }
    }
}