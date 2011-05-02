using System;

namespace DotNetKillboard.Events
{
    /// <summary>
    /// Exception to indicate a change of version while working on a specific aggregate
    /// </summary>
    public class ConcurrencyException : Exception
    {

        /// <summary>
        /// Aggregate identifier
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Aggregate stored version
        /// </summary>
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Aggregate current version
        /// </summary>
        public int ExpectedVersion { get; set; }

        public ConcurrencyException(Guid aggregateId, int currentVersion, int expectedVersion) {
            AggregateId = aggregateId;
            CurrentVersion = currentVersion;
            ExpectedVersion = expectedVersion;
        }
    }
}