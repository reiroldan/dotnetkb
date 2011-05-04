using System.Linq;
using DotNetKillboard.Data;
using DotNetKillboard.Data.Queries;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using MongoDB.Driver;
using NUnit.Framework;

namespace Tests.MongoDataTest
{
    [TestFixture]
    public class ReportingRepositoryTest
    {

        private const string ConnectionString = "server=localhost";
        private const string DataBase = "ReportingRepositoryTest";
        private MongoServer _server;

        [SetUp]
        public void Setup() {
            _server = MongoServer.Create(ConnectionString);
        }

        [Test]
        public void SingleQueryTest() {
            var dto = new ItemDto {
                BasePrice = 100,
                Capacity = 20,
                Description = "Description",
                GroupId = 1,
                Icon = 2,
                Id = 3,
                MarketGroup = 4,
                Mass = 100.23m,
                Name = "An Item",
                PortionSize = 5,
                RaceId = 666,
                Radius = 12312321.213m,
                Volume = 333.444m
            };

            Save(dto);

            var resolver = new TestResolver();
            resolver.Container.Register<IItemByNameQuery, ItemByNameQuery>();
            var mr = new MongoReportingRepository(resolver, ConnectionString, DataBase);
            var result = mr.QueryFor<IItemByNameQuery>(q => q.Name = dto.Name).Execute();

            Assert.AreEqual(result.Name, dto.Name);

            result.Dump();
        }

        [Test]
        public void MultiQueryTest() {
            var dto1 = new ItemDto {
                BasePrice = 1,
                Capacity = 1,
                Description = "Description",
                GroupId = 1,
                Icon = 1,
                Id = 1,
                MarketGroup = 1,
                Mass = 1,
                Name = "Item 1",
                PortionSize = 1,
                RaceId = 1,
                Radius = 1,
                Volume = 2
            };

            Save(dto1);

            var dto2 = new ItemDto {
                BasePrice = 2,
                Capacity = 2,
                Description = "Description",
                GroupId = 2,
                Icon = 2,
                Id = 2,
                MarketGroup = 2,
                Mass = 2,
                Name = "Item 2",
                PortionSize = 2,
                RaceId = 2,
                Radius = 2,
                Volume = 2
            };

            Save(dto2);

            var resolver = new TestResolver();
            resolver.Container.Register<IItemsWithNamesQuery, ItemsWithNamesQuery>();
            var mr = new MongoReportingRepository(resolver, ConnectionString, DataBase);
            var result = mr.QueryFor<IItemsWithNamesQuery>(q => q.Names = new[] { dto1.Name, dto2.Name }).Execute();

            Assert.AreEqual(result.Count(), 2);

            result.Dump();
        }

        [TearDown]
        private void CleanUp() {
            _server.DropDatabase(DataBase);
            _server.Disconnect();
        }

        #region Helpers

        private void Save<T>(T obj) {
            GetCollection<T>().Save(obj);
        }

        private MongoCollection<T> GetCollection<T>() {
            var collection = _server.GetDatabase(DataBase).GetCollection<T>(
               CollectionNamesFactory.GetCollectionNameFromType<T>());
            return collection;
        }

        #endregion
    }
}