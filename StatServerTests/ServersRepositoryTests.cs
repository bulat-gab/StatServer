using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using StatServerCore.Extensions;
using StatServerCore.Model.Mongo;

namespace StatServerTests
{
    public class ServersRepositoryTests : RepositoryTestBase
    {
        private IServersRepository repository;

        private const string Endpoint = "192.168.0.1";

        private static readonly Info Info = new Info
        {
            GameMode = new[] {GameMode.DM, GameMode.TDM},
            Name = "Gamers"
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            repository = new ServersRepository(new StatServerContext(Settings));
        }

        [TearDown]
        public void TearDown()
        {
            MongoClient.GetDatabase(DatabaseName).DropCollection("Servers");
        }

        [Test]
        public async Task GetAllServersInfo_DbEmpty_ShouldReturnNothing()
        {
            var serversInfo = await repository.GetAllServersInfo();
            serversInfo.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllServersInfo_ShouldReturnInfo()
        {
            await repository.SaveServerInfo(Endpoint, Info);

            var serversInfo = await repository.GetAllServersInfo();

            serversInfo.First().Should().BeEquivalentTo(Info);
        }

        [Test]
        public async Task GetServerInfo_ShouldReturn_RequestedServer()
        {
            var endpoint2 = "456";
            var info2 = new Info
            {
                GameMode = new[] {GameMode.DM},
                Name = "Ninjas"
            };

            await repository.SaveServerInfo(Endpoint, Info);
            await repository.SaveServerInfo(endpoint2, info2);

            var serverInfo = await repository.GetServerInfo(endpoint2);

            serverInfo.Should().BeEquivalentTo(info2);
        }

        [Test]
        public async Task GetServerInfo_ShouldReturnNothing_IfEndpointNotFound()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var serversInfo = await repository.GetAllServersInfo();
            serversInfo.Should().NotBeEmpty();

            var serverInfo = await repository.GetServerInfo("wrong_endpoint");
            serverInfo.Should().BeNull();
        }

        [Test]
        public async Task SaveServerInfo_ShouldInsert_Value()
        {
            await repository.SaveServerInfo(Endpoint, Info);

            var serverInfo = await repository.GetServerInfo(Endpoint);
            serverInfo.Should().BeEquivalentTo(Info);
        }

        [Test]
        public async Task GetMatch_ShouldThrow_IfEndpointNotFound()
        {
            Assert.ThrowsAsync<ServerNotFoundException>(async () => await repository.GetMatch("random.endpoint", DateTime.Now));
        }
        
        [Test]
        public async Task GetMatch_ShouldThrow_IfMatchNotFound()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var infoFromDb = await repository.GetServerInfo(Endpoint);
            infoFromDb.Should().BeEquivalentTo(Info);
            
            var timestamp = DateTime.UtcNow.AddDays(-1);
            await repository.SaveMatch(Endpoint, timestamp, new Match());
            
            Assert.ThrowsAsync<MatchNotFoundException>(async () => await repository.GetMatch(Endpoint, DateTime.UtcNow));
        }
        
        [Test]
        public async Task GetMatch_ShouldReturn_WithRequestedTimeStamp()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var infoFromDb = await repository.GetServerInfo(Endpoint);
            infoFromDb.Should().BeEquivalentTo(Info);
            
            var t1 = DateTime.UtcNow.AddDays(-1);
            var m1 = new Match
            {
                FragLimit = 10,
                GameMode = GameMode.TDM,
                Map = "map1",
                Scoreboard = Array.Empty<Score>(),
                TimeElapsed = TimeSpan.FromMinutes(10)
            };
            await repository.SaveMatch(Endpoint, t1, m1);
            
            var t2 = DateTime.UtcNow.AddDays(-2);
            var m2 = new Match
            {
                FragLimit = 15,
                GameMode = GameMode.DM,
                Map = "map2",
                Scoreboard = Array.Empty<Score>(),
                TimeElapsed = TimeSpan.FromMinutes(16)
            };
            await repository.SaveMatch(Endpoint, t2, m2);

            var match = await repository.GetMatch(Endpoint, t2);
            match.Should().BeEquivalentTo(m2);
        }

        [Test]
        public async Task SaveMatch_ShouldThrow_IfEndpointNotFound()
        {
            Assert.ThrowsAsync<ServerNotFoundException>(async () => await repository.SaveMatch("wrong_endpoint", DateTime.Now, new Match()));
        }
        
        [Test]
        public async Task SaveMatch_ShouldReturn_Ok()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var b = await repository.GetServerInfo(Endpoint);
            b.Should().NotBeNull();
            var t = DateTime.UtcNow;
            
            await repository.SaveMatch(Endpoint, t, new Match());

            var match = await repository.GetMatch(Endpoint, t);
            match.Should().NotBeNull();
        }

        [Test]
        public async Task GetServerStats_ShouldThrow_IfEndpointNotFound()
        {
            Assert.ThrowsAsync<ServerNotFoundException>(async () => await repository.GetServerStats("wrong_endpoint"));
        }
        
        [Test]
        public async Task GetServerStats_ShouldReturn_Empty()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var b = await repository.GetServerInfo(Endpoint);
            b.Should().NotBeNull();

            var stats = await repository.GetServerStats(Endpoint);
            stats.IsEmpty().Should().BeTrue();
        }
        
        [Test]
        public async Task GetServerStats_ShouldReturnStats_WhenSingleMatch()
        {
            await repository.SaveServerInfo(Endpoint, Info);
            var b = await repository.GetServerInfo(Endpoint);
            b.Should().NotBeNull();

            var t = DateTime.UtcNow;
            var match = new Match
            {
                FragLimit = 10,
                GameMode = GameMode.DM,
                Map = "CS Assault",
                Scoreboard = new []
                {
                    new Score{Name = "Alek", Kills = 10, Deaths = 0, Frags = 10},
                    new Score{Name = "Bob", Kills = 0, Deaths = 10, Frags = 0},
                    
                }
            };
            await repository.SaveMatch(Endpoint, t, match);
            
            var stats = await repository.GetServerStats(Endpoint);
            stats.TotalMatchesPlayed.Should().Be(1);
            stats.MaximumMatchesPerDay.Should().Be(1);
            stats.AverageMatchesPerDay.Should().Be(1);
            stats.MaximumPopulation.Should().Be(2);
            stats.AveragePopulation.Should().Be(2);
            stats.Top5GameModes.Should().BeEquivalentTo(new[] {GameMode.DM});
            stats.Top5Maps.Should().BeEquivalentTo(new[] {"CS Assault"});
        }
    }
}