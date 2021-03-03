using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence.ClassMaps;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class Context : IContext
    {
        public IMongoCollection<Entity> Entities { get; set; }

        public Context(string connectionString, string databaseName)
        {
            Init();

            var mongoUrl = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            Entities = mongoDatabase.GetCollection<Entity>("entities");
        }

        public void Init()
        {
            BsonClassMap.RegisterClassMap(new AuditableEntityClassMap());
            BsonClassMap.RegisterClassMap(new EntityClassMap());
        }

        public async Task<Envelope<T>> GetEnvelopeAsync<T>(
            IMongoCollection<T> mongoCollection,
            PageParameters pageParameters
        )
        {
            var countPipeline = PipelineDefinition<T, AggregateCountResult>.Create(
                new[] { PipelineStageDefinitionBuilder.Count<T>() }
            );
            var dataPipeLine = PipelineDefinition<T, T>.Create(
                new[]
                {
                    pageParameters.IsAscend
                    ? PipelineStageDefinitionBuilder.Sort (Builders<T>.Sort.Ascending (pageParameters.SortBy))
                    : PipelineStageDefinitionBuilder.Sort (Builders<T>.Sort.Descending (pageParameters.SortBy)),
                    PipelineStageDefinitionBuilder.Skip<T> ((pageParameters.PageNumber - 1) * pageParameters.PageSize),
                    PipelineStageDefinitionBuilder.Limit<T> (pageParameters.PageSize)
                }
            );

            var countFacet = AggregateFacet.Create("count", countPipeline);
            var dataFacet = AggregateFacet.Create("data", dataPipeLine);

            var aggregation = await mongoCollection.Aggregate()
                .Match(Builders<T>.Filter.Empty)
                .Facet(countFacet, dataFacet)
                .FirstOrDefaultAsync();

            var count = aggregation.Facets
                .FirstOrDefault(x => x.Name == "count")
                .Output<AggregateCountResult>()
                .FirstOrDefault()
                .Count;

            var data = aggregation.Facets
                .FirstOrDefault(x => x.Name == "data")
                .Output<T>()
                .ToList();

            var envelope = new Envelope<T>()
            {
                Items = data,
                Count = count
            };

            return envelope;
        }
    }
}