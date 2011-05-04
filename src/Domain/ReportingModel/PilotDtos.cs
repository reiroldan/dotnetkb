using System;

namespace DotNetKillboard.ReportingModel
{
    public class PilotDto
    {
        public Guid Id { get; set; }

        public int AllianceId { get; set; }

        public int CorporationId { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }
    }
}