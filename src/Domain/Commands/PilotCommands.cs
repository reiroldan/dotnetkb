using System;

namespace DotNetKillboard.Commands
{
    public class CreatePilot : CommandBase
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public int CorporationId { get; set; }

        public int ExternalId { get; set; }

        public int AllianceId { get; set; }

        public CreatePilot(Guid id, int sequence, string name, int allianceId, int corporationId, int externalId)
            : base(id) {
            Sequence = sequence;
            Name = name;
            AllianceId = allianceId;
            CorporationId = corporationId;
            ExternalId = externalId;
        }
    }

    public class ChangePilotsCorporation : CommandBase
    {
        public int CorporationId { get; set; }

        public ChangePilotsCorporation(Guid id, int corporationId)
            : base(id) {
            CorporationId = corporationId;
        }
    }

    public class ChangePilotsAlliance : CommandBase
    {
        public int AllianceId { get; set; }

        public ChangePilotsAlliance(Guid id, int allianceId)
            : base(id) {
            AllianceId = allianceId;
        }        
    }
}