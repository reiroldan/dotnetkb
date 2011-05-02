using System;
using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.Bus;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data
{
    public class MongoEventStore : IEventStore
    {
        private readonly IBus _bus;
        private readonly string _database;
        private readonly MongoServer _instance;

        public MongoEventStore(IBus bus, string connectionString, string database = "EventStore") {
            _bus = bus;
            _database = database;
            _instance = MongoServer.Create(connectionString);
        }

        public void SaveEvents(AggregateRoot aggregate, IEnumerable<IEvent> events) {
            var col = GetSourceCollection();
            var eventSource = col.FindOne(Query.EQ("_id", aggregate.Id));
            var currentVersion = aggregate.Version;

            if (eventSource == null) {
                eventSource = new EventSource(aggregate.Id, currentVersion);
            } else if (eventSource.Version != currentVersion && currentVersion != -1) {
                throw new ConcurrencyException(aggregate.Id, eventSource.Version, currentVersion);
            }

            var i = currentVersion;

            foreach (var @event in events) {
                i++;
                @event.Version = i;
                eventSource.Add(new EventDescriptor(@event, DateTime.UtcNow));
                eventSource.Version = i;
                _bus.Publish(@event);
            }

            col.Save(eventSource);
        }

        public List<IEvent> GetEventsForAggregate(Guid aggregateId) {
            var col = GetSourceCollection();
            var eventSource = col.FindOne(Query.EQ("_id", aggregateId));

            if (eventSource == null)
                throw new AggregateNotFoundException();

            return eventSource.Events.Select(e => e.Event).ToList();
        }

        private MongoCollection<EventSource> GetSourceCollection() {
            var db = _instance.GetDatabase(_database);
            var col = db.GetCollection<EventSource>("EventSources");
            return col;
        }

        #region Helper Classes

        public class EventSource
        {
            public Guid Id { get; set; }
            public int Version { get; set; }
            public List<EventDescriptor> Events { get; set; }

            public EventSource(Guid id, int version) {
                Id = id;
                Version = version;
                Events = new List<EventDescriptor>();
            }

            public void Add(EventDescriptor eventDescriptor) {
                Events.Add(eventDescriptor);
            }
        }

        public class EventDescriptor
        {
            public DateTime Timestamp { get; set; }
            public IEvent Event { get; set; }

            public EventDescriptor(IEvent @event, DateTime timestamp) {
                Event = @event;
                Timestamp = timestamp;
            }
        }

        #endregion
    }
}