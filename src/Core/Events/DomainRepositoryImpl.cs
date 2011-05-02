using System;
using DotNetKillboard.Domain;

namespace DotNetKillboard.Events
{

    /// <summary>
    /// Simple domain repository implementation
    /// </summary>
    public class DomainRepositoryImpl : IDomainRepository
    {
        private readonly IEventStore _storage;

        public DomainRepositoryImpl(IEventStore storage) {
            _storage = storage;
        }

        public void Save<T>(T aggregate) where T : AggregateRoot {
            _storage.SaveEvents(aggregate, aggregate.GetUncommittedChanges());
        }

        public T GetById<T>(Guid id) where T : AggregateRoot, new() {
            var obj = new T();
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}