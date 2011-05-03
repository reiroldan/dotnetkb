using System;

namespace DotNetKillboard.Events
{
    public class AllianceCreated : EventBase
    {
        public string Name { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public AllianceCreated(Guid id, string name, int externalId, DateTime timestamp)
            : base(id) {
            Name = name;
            ExternalId = externalId;
            Timestamp = timestamp;
        }
    }
}