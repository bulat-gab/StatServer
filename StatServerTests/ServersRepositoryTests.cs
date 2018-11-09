using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using StatServerCore.Model.DtoContracts;
using StatServerCore.Model.Mongo;

namespace StatServerTests
{
    public class ServersRepositoryTests : RepositoryTestBase
    {
        private IServersRepository repository;
        
        private const string Endpoint = "123";
        private readonly Info info = new Info
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
            await repository.SaveServerInfo(Endpoint, info);

            var serversInfo = await repository.GetAllServersInfo();
            
            serversInfo.First().Should().BeEquivalentTo(info);
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

            await repository.SaveServerInfo(Endpoint, info);
            await repository.SaveServerInfo(endpoint2, info2);

            var serverInfo = await repository.GetServerInfo(endpoint2);
            
            serverInfo.Should().BeEquivalentTo(info2);
        }
        
        [Test]
        public async Task GetServerInfo_ShouldReturnNothing_WhenEndpointNotFound()
        {
            await repository.SaveServerInfo(Endpoint, info);
            var serversInfo = await repository.GetAllServersInfo();
            serversInfo.Should().NotBeEmpty();

            var serverInfo = await repository.GetServerInfo("wrong_endpoint");
            serverInfo.Should().BeNull();
        }
        
        [Test]
        public async Task SaveServerInfo_ShouldInsert_Value()
        {
            var serversInfo = await repository.GetAllServersInfo();
            serversInfo.Should().BeEmpty();
            await repository.SaveServerInfo(Endpoint, info);
            
            serversInfo = await repository.GetAllServersInfo();
            serversInfo.First().Should().BeEquivalentTo(info);

        }
        
        [Test]
        public async Task _()
        {
            
        }
/*

        [Test]
        public async Task _()
        {
            
        }
        
*/
    }
}