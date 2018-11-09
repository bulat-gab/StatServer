using MongoDB.Driver;

namespace StatServerCore.Model.Mongo
{
    public interface IStatServerContext
    {
        IMongoCollection<ServerEntity> Servers { get; }
    }
}