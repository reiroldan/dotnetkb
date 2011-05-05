using System;
using DotNetKillboard;
using DotNetKillboard.Bus;
using DotNetKillboard.Data;
using DotNetKillboard.Events;
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
            Resolver.Container.Register<IEventStore>(new MongoEventStore(Resolver.Resolve<IBus>(), ConnectionString,
                                                                         DataBase));
            Resolver.Container.Register<IDomainRepository, DomainRepositoryImpl>();
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

        public MongoTestBase RegisterFilters() {
            var filters = FilterHelper.GetFilterTypes();

            foreach (var f in filters) {
                Resolver.Container.Register(f.Key, f.Value);
            }
            return this;
        }

        public MongoTestBase RegisterCommandsAndEvents() {
            var commands = CommandEventConfigHelper.GetCommandAndHandlerTypes();
            var events = CommandEventConfigHelper.GetEventAndHandlerTypes();
            var bus = Resolver.Resolve<IBus>();

            foreach (var pair in commands) {
                bus.RegisterCommand(pair.Key, pair.Value);
            }

            foreach (var pair in events) {
                foreach (var e in pair.Value) {
                    bus.RegisterEvent(pair.Key, e);
                }
            }

            return this;
        }

        #region Helpers

        protected T Resolve<T>() where T : class {
            return Resolver.Container.Resolve<T>();
        }

        protected void Save<T>(T obj) {
            GetCollection<T>().Save(obj);
        }

        protected MongoCollection<T> GetCollection<T>() {
            var collection = MongoServer.GetDatabase(DataBase).GetCollection<T>(
               CollectionNamesFactory.GetCollectionNameFromType<T>());
            return collection;
        }

        #endregion
    }
}