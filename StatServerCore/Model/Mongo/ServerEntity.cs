using MongoDB.Bson.Serialization.Attributes;
using StatServerCore.Model.DtoContracts;

namespace StatServerCore.Model.Mongo
{
    public class ServerEntity
    {
        [BsonId]
        public string Endpoint { get; set; }

        public Info Info { get; set; }

        public MatchEntity[] Matches { get; set; }
    }
}