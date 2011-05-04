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

        private TestResolver _resolver;
        private IBus _bus;
        private IEventStore _eventStore;
        private IDomainRepository _domainRepository;
        private IReportingRepository _reportingRepository;

        [SetUp]
        public void Setup() {
            _resolver = new TestResolver();
            _bus = new InMemoryBus(_resolver);
            _eventStore = new MongoEventStore(_bus, "server=localhost", "TestEventStore");
            _domainRepository = new DomainRepositoryImpl(_eventStore);
            _reportingRepository = new MongoReportingRepository(_resolver, "server=localhost", "TestReporting");

            _resolver.Register(typeof(IDomainRepository), _domainRepository);
            _resolver.Register(typeof(IReportingRepository), _reportingRepository);
        }

        [Test]
        public void Test() {
            var id = Guid.NewGuid();

            _bus.RegisterCommand<TestCreateEntity, TestEntityCommandHandlers>();
            _bus.RegisterCommand<TestChangeEntityName, TestEntityCommandHandlers>();

            _bus.RegisterEvent<TestEntityCreatedEvent, TestEntityEventHandlers>();
            _bus.RegisterEvent<TestEntityNameChangedEvent, TestEntityEventHandlers>();

            _bus.Send(new TestCreateEntity(id, "EntityCreated"));
            _bus.Send(new TestChangeEntityName(id, "Name Changed"));
        }

        #region Domain

        class TestEntity : AggregateRoot
        {
            private string _name;

            public TestEntity() { }

            public TestEntity(Guid id, string name) {
                ApplyChange(new TestEntityCreatedEvent(id, name));
            }

            protected void OnTestEntityCreatedEvent(TestEntityCreatedEvent @event) {
                Id = @event.Id;
                _name = @event.Name;
            }

            protected void OnTestEntityNameChangedEvent(TestEntityNameChangedEvent @event) {
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
            public string Name { get; set; }

            public TestEntityCreatedEvent(Guid id, string name)
                : base(id) {
                Id = id;
                Name = name;
            }
        }

        class TestEntityNameChangedEvent : EventBase
        {
            public string Name { get; set; }

            public TestEntityNameChangedEvent(Guid id, string name)
                : base(id) {
                Id = id;
                Name = name;
            }
        }

        #endregion

        #region Command Handlers

        class TestEntityCommandHandlers : ICommandHandler<TestCreateEntity>, ICommandHandler<TestChangeEntityName>
        {
            private readonly IDomainRepository _domainRepository;

            public TestEntityCommandHandlers(IDomainRepository domainRepository) {
                _domainRepository = domainRepository;
            }

            public void Handle(TestCreateEntity command) {
                var item = new TestEntity(command.Id, command.Name);
                _domainRepository.Save(item);
            }

            public void Handle(TestChangeEntityName command) {
                var item = _domainRepository.GetById<TestEntity>(command.Id);
                item.ChangeName(command.NewName);
                _domainRepository.Save(item);
            }
        }

        #endregion

        #region Event Handlers

        class TestEntityEventHandlers : IEventHandler<TestEntityCreatedEvent>, IEventHandler<TestEntityNameChangedEvent>
        {
            private readonly IReportingRepository _repository;

            public TestEntityEventHandlers(IReportingRepository repository) {
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
