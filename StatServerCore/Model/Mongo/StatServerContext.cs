using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace StatServerCore.Model.Mongo
{
    public class StatServerContext : IStatServerContext
    {
        private readonly IMongoDatabase database;

        public StatServerContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<ServerEntity> Servers => database.GetCollection<ServerEntity>("Servers");
    }
}