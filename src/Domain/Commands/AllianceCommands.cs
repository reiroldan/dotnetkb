using System;

namespace DotNetKillboard.Commands
{
    public class CreateAlliance : CommandBase
    {
        public string Name { get; set; }

        public int ExternalId { get; set; }

        public CreateAlliance(Guid id, string name, int externalId)
            : base(id) {
            Name = name;
            ExternalId = externalId;
        }
    }
}