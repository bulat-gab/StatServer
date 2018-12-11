using System;
using System.Threading.Tasks;
using Contracts;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new StatServerClient();

            var endpoint = "192.168.100.133";
            var info = new Info
            {
                GameMode = new[] {GameMode.DM},
                Name = "Server 1"
            };

//            var result = await client.SaveServerInfo(endpoint, info);
//            Console.WriteLine(result);
            
            var t1 = new DateTime(2018, 11, 15, 11, 17, 16, DateTimeKind.Utc);
//            var m1 = new Match
//            {
//                FragLimit = 10,
//                GameMode = GameMode.DM,
//                Map = "De Dust2",
//                Scoreboard = new []
//                {
//                    new Score{Name = "Alek", Kills = 10, Deaths = 0, Frags = 10},
//                    new Score{Name = "Bob", Kills = 0, Deaths = 10, Frags = 0},
//                    
//                }
//            };
//            await client.SaveMatch(endpoint, t1, m1);

//            var match = await client.GetMatch(endpoint, t1);

            var stats = await client.GetServerStats(endpoint);
        }
    }
}