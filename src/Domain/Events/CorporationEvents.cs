using System;

namespace DotNetKillboard.Events
{
    public class CorporationCreated : EventBase
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public CorporationCreated(Guid id)
            : base(id) {
        }

        public CorporationCreated(Guid id, int sequence, string name, int allianceId, int externalId, DateTime timestamp)
            : base(id) {
            Sequence = sequence;
            Name = name;
            AllianceId = allianceId;
            ExternalId = externalId;
            Timestamp = timestamp;
        }
    }

    public class CorporationAllianceChanged : AsyncEventBase
    {
        public int AllianceId { get; set; }

        public DateTime Timestamp { get; set; }

        public CorporationAllianceChanged(Guid id, int allianceId, DateTime timestamp)
            : base(id) {
            AllianceId = allianceId;
            Timestamp = timestamp;
        }
    }
}