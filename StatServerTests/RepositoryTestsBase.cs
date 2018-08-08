using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;
using StatServerCore.Model;

namespace StatServerTests
{
    public class RepositoryTestBase
    {
        protected string DatabaseName;

        protected IOptions<DbSettings> Settings;

        protected IMongoClient MongoClient;
        private const string ConnectionUrl = "mongodb://localhost/?retryWrites=true";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DatabaseName = string.Concat("Test", Guid.NewGuid().ToString("D").Substring(0, 8));

            MongoClient = new MongoClient(ConnectionUrl);

            Settings = Options.Create(new DbSettings
            {
                ConnectionString = ConnectionUrl,
                Database = DatabaseName
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine(DatabaseName);
//            mongoClient.DropDatabase(databaseName);
        }
    }
}