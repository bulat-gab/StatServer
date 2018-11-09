using System;
using MongoDB.Bson.Serialization.Attributes;
using StatServerCore.Model.DtoContracts;

namespace StatServerCore.Model.Mongo
{
    public class MatchEntity
    {
        [BsonId]
        public DateTime Timestamp { get; set; }

        public Match Match;
    }
}