using System;

namespace DotNetKillboard.Services.Model
{
    public class ParsedKillHeader
    {
        public DateTime? Timestamp { get; set; }

        public string VictimName { get; set; }

        public string CorporationName { get; set; }

        public string AllianceName { get; set; }

        public string FactionName { get; set; }

        public string ShipName { get; set; }

        public string SystemName { get; set; }

        public decimal SystemSecurity { get; set; }

        public decimal DamageTaken { get; set; }
    }
}