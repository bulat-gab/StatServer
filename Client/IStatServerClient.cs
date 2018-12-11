using System;
using System.Threading.Tasks;
using Contracts;

namespace Client
{
    public interface IStatServerClient
    {
        Task<Info[]> GetAllServersInfo();
        
        Task<Info> GetServerInfo(string endpoint);
        
        Task<string> SaveServerInfo(string endpoint, Info info);

        Task<Match> GetMatch(string endpoint, DateTime timestamp);

        Task<string> SaveMatch(string endpoint, DateTime timestamp, Match match);
        
        Task<ServerStats> GetServerStats(string endpoint);

        Task<PlayerStats> GetPlayersStats(string name);

        Task<object> GetRecentMatches(int count = 5);
        
        Task<object> GetBestPlayers(int count = 5);
        
        Task<object> GetPopularServers(int count = 5);
    }
}