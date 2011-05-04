using DotNetKillboard;
using DotNetKillboard.Bus;
using DotNetKillboard.Data;
using DotNetKillboard.Reporting;
using DotNetKillboard.Services;
using DotNetKillboard.Services.Implementation;
using MongoDB.Driver;
using NUnit.Framework;

namespace Tests
{
    public abstract class MongoTestBase
    {

        private const string ConnectionString = "server=localhost";
        private const string DataBase = "ReportingRepositoryTest";
        private bool _busRegistered;

        protected MongoServer MongoServer { get; private set; }

        protected TestResolver Resolver { get; private set; }

        [SetUp]
        public void Setup() {
            MongoServer = MongoServer.Create(ConnectionString);
            MongoServer.DropDatabase(DataBase);
            Resolver = new TestResolver();
            OnSetup();
        }

        [TearDown]
        private void CleanUp() {
            MongoServer.DropDatabase(DataBase);
            MongoServer.Disconnect();
            OnCleanUp();
        }

        protected virtual void OnSetup() { }

        protected virtual void OnCleanUp() { }

        public MongoTestBase RegisterBus() {
            _busRegistered = true;
            Resolver.Container.Register<IResolver>(Resolver);
            Resolver.Container.Register<IBus, InMemoryBus>();
            return this;
        }

        public MongoTestBase RegisterReportingRepository() {
            var repos = new MongoReportingRepository(Resolver, ConnectionString, DataBase);
            Resolver.Container.Register<IReportingRepository>(repos);
            return this;
        }

        public MongoTestBase RegisterEntitiesServices() {
            if (!_busRegistered)
                RegisterBus();

            Resolver.Container.Register<IEntitiesService, EntitesServiceImpl>();
            return this;
        }

        protected T Resolve<T>() where T : class {
            return Resolver.Container.Resolve<T>();
        }

        
    }
}