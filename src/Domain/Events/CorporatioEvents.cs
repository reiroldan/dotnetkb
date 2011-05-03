using System;

namespace DotNetKillboard.Events
{
    public class CorporatioCreated : EventBase
    {
        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public CorporatioCreated(Guid id)
            : base(id) {
        }

        public CorporatioCreated(Guid id, string name, int allianceId, int externalId, DateTime timestamp)
            : base(id) {
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

        public CorporationAllianceChanged(Guid id, int allianceId, DateTime timestamp) : base(id) {
            AllianceId = allianceId;
            Timestamp = timestamp;
        }
    }
}