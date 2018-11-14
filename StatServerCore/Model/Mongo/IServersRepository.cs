using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;

namespace StatServerCore.Model.Mongo
{
    public interface IServersRepository
    {
        Task<IEnumerable<Info>> GetAllServersInfo();

        Task<Info> GetServerInfo(string endpoint);

        Task SaveServerInfo(string endpoint, Info info);

        Task<Match> GetMatch(string endpoint, DateTime timestamp);

        Task SaveMatch(string endpoint, DateTime timestamp, Match match);

        Task<ServerStats> GetServerStats(string endpoint);

        // TODO: add other methods
    }
}