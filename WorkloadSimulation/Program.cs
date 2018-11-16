using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Client;
using Contracts;

namespace WorkloadSimulation
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private static readonly StatServerClient Client = new StatServerClient();
        private static readonly Random RNG = new Random();

        private static async Task Main(string[] args)
        {
//            GenerateServers();
//            Client.SaveServerInfo("e", new Info());

            var r = await Client.GetAllServersInfo();
        }

        public static async Task GenerateServers()
        {
            var endpoints = GetEndpoints();

            var serverNames = GetServerNames();

            foreach (var (endpoint, serverName) in endpoints.Zip(serverNames, (s, s1) => (s, s1)))
            {
                var info = new Info
                {
                    Name = serverName,
                    GameMode = GenerateGameModes()
                };
                var r = await Client.SaveServerInfo(endpoint, info);
            }
        }

        private static List<string> GetServerNames()
        {
            var serverNames = new List<string>();
            using (var sr = File.OpenText(@"./Data/Servers.txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    serverNames.Add(s);
                }
            }

            return serverNames;
        }

        private static List<string> GetEndpoints()
        {
            var endpoints = new List<string>();
            using (var sr = File.OpenText(@"./Data/Ips.txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    endpoints.Add(s);
                }
            }

            return endpoints;
        }

        private static GameMode[] GenerateGameModes()
        {
            var gameModesCount = Enum.GetValues(typeof(GameMode)).Length;

            var modes = new List<GameMode>(gameModesCount);
            for (var i = 0; i < RNG.Next(gameModesCount); i++)
            {
                var randomMode = RandomEnumValue<GameMode>();
                if (!modes.Contains(randomMode))
                {
                    modes.Add(randomMode);
                }
            }

            return modes.ToArray();
        }

        private static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T) v.GetValue(RNG.Next(v.Length));
        }
    }
}