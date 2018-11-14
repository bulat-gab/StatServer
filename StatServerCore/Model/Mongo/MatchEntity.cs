using System;
using Contracts;
using MongoDB.Bson.Serialization.Attributes;

namespace StatServerCore.Model.Mongo
{
    public class MatchEntity
    {
        [BsonId]
        public DateTime Timestamp { get; set; }

        public Match Match;
    }
}