using System;

namespace DotNetKillboard.Events
{
    public class PilotCreated : EventBase
    {
        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int CorporationId { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }

        public PilotCreated(Guid id, string name, int allianceId, int corporationId, int externalId, DateTime timestamp)
            : base(id) {
            Name = name;
            AllianceId = allianceId;
            CorporationId = corporationId;
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
}