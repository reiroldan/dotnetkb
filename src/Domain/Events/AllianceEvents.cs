using System;

namespace DotNetKillboard.Events
{
    public class AllianceCreated : EventBase
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public AllianceCreated(Guid id, int sequence, string name, int externalId, DateTime timestamp)
            : base(id) {
            Sequence = sequence;
            Name = name;
            ExternalId = externalId;
            Timestamp = timestamp;
        }
    }
}