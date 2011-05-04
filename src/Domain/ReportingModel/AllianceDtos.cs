using System;

namespace DotNetKillboard.Reporting
{
    public class AllianceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ExternalId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}