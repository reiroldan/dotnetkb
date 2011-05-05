using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetKillboard.Events;

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
        /// Apply the event
        /// </summary>
        /// <param name="event"></param>
        /// <param name="isNew"></param>
        public void ApplyChange(IEvent @event, bool isNew = true) {
            var targetType = GetType();
            var methodsToMatch = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var expectedName = string.Format("On{0}", @event.GetType().Name);

            foreach (var method in methodsToMatch) {

                if (string.Compare(method.Name, expectedName, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != 1 || !typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType))
                    continue;

                method.Invoke(this, new[] { @event });

                if (isNew)
                    _changes.Add(@event);

                return;
            }

            throw new Exception(string.Format("Could not find a method named {0} on type {1}", expectedName, GetType().Name));
        }
    }
}