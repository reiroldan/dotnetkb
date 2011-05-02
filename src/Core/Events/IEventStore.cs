using System;
using System.Collections.Generic;
using DotNetKillboard.Domain;

namespace DotNetKillboard.Events
{
    /// <summary>
    /// Event storage/retrieval mechanism
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Persists a list of changes for a given aggregate
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="events"></param>
        void SaveEvents(AggregateRoot aggregate, IEnumerable<IEvent> events);

        /// <summary>
        /// Retrieve all events for a given aggregate id
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        List<IEvent> GetEventsForAggregate(Guid aggregateId);
    }
}