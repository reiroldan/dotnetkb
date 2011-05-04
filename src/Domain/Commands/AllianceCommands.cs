using System;

namespace DotNetKillboard.Commands
{
    public class CreateAlliance : CommandBase
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public int ExternalId { get; set; }

        public CreateAlliance(Guid id, int sequence, string name, int externalId)
            : base(id) {
            Sequence = sequence;
            Name = name;
            ExternalId = externalId;
        }
    }
}