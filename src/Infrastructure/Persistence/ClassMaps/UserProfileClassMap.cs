using Domain.Entities;
using MongoDB.Bson.Serialization;

namespace Infrastructure.Persistence.ClassMaps
{
    public class UserProfileClassMap : BsonClassMap<UserProfile>
    {
        public UserProfileClassMap()
        {
            AutoMap();
        }
    }
}