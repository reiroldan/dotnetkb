using System;
using DotNetKillboard.Bus;
using DotNetKillboard.Commands;
using DotNetKillboard.Data;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;
using DotNetKillboard.Reporting;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class InMemoryBusTest
    {

        private IBus _bus;
        private IEventStore _eventStore;
        private IDomainRepository _domainRepository;
        private IReportingRepository _reportingRepository;

        [SetUp]
        public void Setup() {
            _bus = new InMemoryBus();
            _eventStore = new MongoEventStore(_bus, "server=localhost", "TestEventStore");
            _domainRepository = new DomainRepositoryImpl(_eventStore);
            _reportingRepository = new MongoReportingRepository("server=localhost", "TestReporting");
        }

        [Test]
        public void Test() {
            var id = Guid.NewGuid();
            _bus.Register<TestCreateEntity>(HandleCreateCommand);
            _bus.Register<TestChangeEntityName>(HandleChangeName);

            var handler = new TestEntityHandlers(_reportingRepository);
            _bus.Register<TestEntityCreatedEvent>(handler.Handle);
            _bus.Register<TestEntityNameChangedEvent>(handler.Handle);

            _bus.Send(new TestCreateEntity(id, "EntityCreated"));
            _bus.Send(new TestChangeEntityName(id, "Name Changed"));
        }

        private void HandleChangeName(TestChangeEntityName cmd) {
            var item = _domainRepository.GetById<TestEntity>(cmd.Id);
            item.ChangeName(cmd.NewName);
            _domainRepository.Save(item);
        }

        private void HandleCreateCommand(TestCreateEntity cmd) {
            var item = new TestEntity(cmd.Id, cmd.Name);
            _domainRepository.Save(item);
        }

        #region Domain

        class TestEntity : AggregateRoot
        {
            private string _name;

            public TestEntity() { }

            public TestEntity(Guid id, string name) {
                ApplyChange(new TestEntityCreatedEvent(id, name));
            }

            protected void Apply(TestEntityCreatedEvent @event) {
                Id = @event.Id;
                _name = @event.Name;
            }

            public void ChangeName(string name) {
                ApplyChange(new TestEntityNameChangedEvent(Id, name));
            }
        }

        #endregion

        #region Commands

        class TestCreateEntity : CommandBase
        {
            public string Name { get; private set; }

            public TestCreateEntity(Guid id, string name)
                : base(id) {
                Name = name;
            }
        }

        class TestChangeEntityName : CommandBase
        {
            public string NewName { get; set; }

            public TestChangeEntityName(Guid id, string newName)
                : base(id) {
                NewName = newName;
            }
        }

        #endregion

        #region Events

        class TestEntityCreatedEvent : EventBase
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public TestEntityCreatedEvent(Guid id, string name) {
                Id = id;
                Name = name;
            }
        }

        class TestEntityNameChangedEvent : EventBase
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public TestEntityNameChangedEvent(Guid id, string name) {
                Id = id;
                Name = name;
            }
        }

        #endregion

        #region Event Handlers

        class TestEntityHandlers : IEventHandler<TestEntityCreatedEvent>, IEventHandler<TestEntityNameChangedEvent>
        {
            private readonly IReportingRepository _repository;

            public TestEntityHandlers(IReportingRepository repository) {
                _repository = repository;
            }

            public void Handle(TestEntityCreatedEvent @event) {
                _repository.Save(new TestEntityReport { Id = @event.Id, Name = @event.Name });
            }

            public void Handle(TestEntityNameChangedEvent @event) {
                var r = _repository.Get<TestEntityReport>(@event.Id);
                r.Name = @event.Name;
                _repository.Save(r);
            }
        }

        #endregion

        #region Reporting

        class TestEntityReport
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

        }

        #endregion
    }
}
