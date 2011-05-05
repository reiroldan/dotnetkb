using System;

namespace DotNetKillboard.Events
{
    public class PilotCreated : EventBase
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int CorporationId { get; set; }

        public decimal SecurityStatus { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public PilotCreated(Guid id, int sequence, string name, int allianceId, int corporationId, decimal securityStatus, int externalId, DateTime timestamp)
            : base(id) {
            Sequence = sequence;
            Name = name;
            AllianceId = allianceId;
            CorporationId = corporationId;
            SecurityStatus = securityStatus;
            ExternalId = externalId;
            Timestamp = timestamp;
        }
    }

    public class PilotCorporationChanged : AsyncEventBase
    {
        public int CorporationId { get; set; }

        public DateTime Timestamp { get; set; }

        public PilotCorporationChanged(Guid id, int corporationId, DateTime timestamp)
            : base(id) {
            CorporationId = corporationId;
            Timestamp = timestamp;
        }
    }

    public class PilotAllianceChanged : AsyncEventBase
    {
        public int AllianceId { get; set; }

        public DateTime Timestamp { get; set; }

        public PilotAllianceChanged(Guid id, int allianceId, DateTime timestamp) : base(id) {
            AllianceId = allianceId;
            Timestamp = timestamp;
        }
    }
}