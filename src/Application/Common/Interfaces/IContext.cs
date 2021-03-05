using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;
using MongoDB.Driver;

namespace Application.Common.Interfaces
{
    public interface IContext
    {
        IMongoCollection<Entity> Entities { get; set; }
        IMongoCollection<UserProfile> UserProfiles { get; set; }

        Task<Envelope<T>> GetEnvelopeAsync<T>(
            IMongoCollection<T> collection,
            PageParameters pageParameters
        );
    }
}