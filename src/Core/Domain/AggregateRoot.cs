using System;
using System.Collections.Generic;
using DotNetKillboard.Events;
using DotNetKillboard.Utils;

namespace DotNetKillboard.Domain
{

    /// <summary>
    /// Base class for all Aggregate roots
    /// </summary>
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Aggregate root identifier
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Version number of this aggregate root
        /// </summary>
        public int Version { get; internal set; }

        /// <summary>
        /// Retrieve a list of events which haven't been commited yet
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEvent> GetUncommittedChanges() {
            return _changes;
        }

        /// <summary>
        /// Restores the aggregate root from its history
        /// </summary>
        /// <param name="history"></param>
        public void LoadsFromHistory(IEnumerable<IEvent> history) {
            foreach (var e in history) {
                ApplyChange(e, false);
                Version = e.Version;
            }
        }

        /// <summary>
        /// Apply a single event
        /// </summary>
        /// <param name="event"></param>
        /// <param name="isNew">If the event is new, it will be added to the pending changes list</param>
        public void ApplyChange(IEvent @event, bool isNew = true) {
            this.AsDynamic().Apply(@event);

            if (isNew)
                _changes.Add(@event);
        }
    }
}