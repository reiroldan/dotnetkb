using System;

namespace DotNetKillboard.Domain
{
    public class Corporation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AllianceId { get; set; }

        public int ExternalId { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}