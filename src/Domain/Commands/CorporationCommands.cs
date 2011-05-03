using System;

namespace DotNetKillboard.Commands
{
    public class CreateCorporation : CommandBase
    {
        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int ExternalId { get; set; }

        public CreateCorporation(Guid id, string name, int allianceId, int externalId)
            : base(id) {
            Name = name;
            AllianceId = allianceId;
            ExternalId = externalId;
        }
    }

    public class ChangeCorporationsAlliance : CommandBase
    {
        public int AllianceId { get; set; }

        public ChangeCorporationsAlliance(Guid id, int allianceId) : base(id) {
            AllianceId = allianceId;
        }
    }
}