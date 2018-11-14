using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.Exceptions;
using MongoDB.Driver;
using StatServerCore.Extensions;

namespace StatServerCore.Model.Mongo
{
    public class ServersRepository : IServersRepository
    {
        private readonly IMongoCollection<ServerEntity> servers;

        public ServersRepository(IStatServerContext context) => servers = context.Servers;

        public async Task<IEnumerable<Info>> GetAllServersInfo()
        {
            return await servers.Find(_ => true)
                                .Project(x => x.Info)
                                .ToListAsync();
        }

        public Task<Info> GetServerInfo(string endpoint)
        {
            var filter = Builders<ServerEntity>.Filter.Eq(x => x.Endpoint, endpoint);

            return servers.Find(filter)
                          .Project(x => x.Info)
                          .FirstOrDefaultAsync();
        }

        public async Task SaveServerInfo(string endpoint, Info info)
        {
            var server = new ServerEntity
            {
                Endpoint = endpoint,
                Info = info,
                Matches = Array.Empty<MatchEntity>()
            };

            await servers.InsertOneAsync(server);
        }

        public async Task<Match> GetMatch(string endpoint, DateTime timestamp)
        {
            var server = await servers.Find(x => x.Endpoint.Equals(endpoint))
                                      .FirstOrDefaultAsync();

            if (server == null)
            {
                throw new ServerNotFoundException(endpoint);
            }

            var matchEntity = server.Matches.FirstOrDefault(x => timestamp.CompareWith(x.Timestamp));

            if (matchEntity == null)
            {
                throw new MatchNotFoundException(timestamp.ToString("U"));
            }
            
            return matchEntity.Match;
        }

        public async Task SaveMatch(string endpoint, DateTime timestamp, Match match)
        {
            var matchEntity = new MatchEntity
            {
                Timestamp = timestamp,
                Match = match
            };
            var update = Builders<ServerEntity>.Update.Push(x => x.Matches, matchEntity);

            var result = await servers.UpdateOneAsync(x => x.Endpoint.Equals(endpoint), update);
            if (!result.IsAcknowledged || result.ModifiedCount <= 0)
            {
                throw new ServerNotFoundException(endpoint);
            }
        }

        public async Task<ServerStats> GetServerStats(string endpoint)
        {
            var serverEntity = await servers.Find(x => x.Endpoint.Equals(endpoint))
                                            .FirstOrDefaultAsync();
            if (serverEntity == null)
            {
                throw new ServerNotFoundException(endpoint);
            }

            var matches = serverEntity.Matches;
            if (matches.Length == 0)
            {
                return ServerStats.CreateEmpty();
            }
            
            var grouped = matches.GroupBy(x => x.Timestamp.Date).ToArray();
            var maxPerDay = grouped.OrderByDescending(x => x.Count()).First().Count();
            var averagePerDay = grouped.Average(x => x.Count());

            var maxPopulation = matches.Max(x => x.Match.Scoreboard.Length);
            var averagePopulation = matches.Average(x => x.Match.Scoreboard.Length);
            var top5GameModes = matches.GroupBy(x => x.Match.GameMode)
                                       .OrderByDescending(x => x.Count())
                                       .Take(5)
                                       .Select(x => x.Key)
                                       .ToArray();
            var top5Maps = matches.GroupBy(x => x.Match.Map)
                                  .OrderByDescending(x => x.Count())
                                  .Take(5)
                                  .Select(x => x.Key)
                                  .ToArray();

            var stats = new ServerStats
            {
                TotalMatchesPlayed = matches.Length,
                MaximumMatchesPerDay = maxPerDay,
                AverageMatchesPerDay = averagePerDay,
                MaximumPopulation = maxPopulation,
                AveragePopulation = averagePopulation,
                Top5GameModes = top5GameModes,
                Top5Maps = top5Maps
            };

            return stats;
        }

        public Task<PlayerStats> GetPlayersStats(string name) => throw new NotImplementedException();
    }
}