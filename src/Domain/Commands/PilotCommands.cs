using System;

namespace DotNetKillboard.Commands
{
    public class CreatePilot : CommandBase
    {
        public string Name { get; set; }

        public int CorporationId { get; set; }

        public DateTime Timestamp { get; set; }

        public int ExternalId { get; set; }

        public int AllianceId { get; set; }

        public CreatePilot(Guid id, string name, int allianceId, int corporationId, DateTime timestamp, int externalId)
            : base(id) {
            Name = name;
            AllianceId = allianceId;
            CorporationId = corporationId;
            Timestamp = timestamp;
            ExternalId = externalId;
        }
    }

    public class ChangePilotsAllianceAndCorporation : CommandBase
    {
        public int AllianceId { get; set; }

        public int CorporationId { get; set; }

        public DateTime Timestamp { get; set; }

        public ChangePilotsAllianceAndCorporation(Guid id, int allianceId, int corporationId, DateTime timestamp)
            : base(id) {
            AllianceId = allianceId;
            CorporationId = corporationId;
            Timestamp = timestamp;
        }
    }
}