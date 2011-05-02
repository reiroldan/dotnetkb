using System;

namespace DotNetKillboard.Domain
{
    public class Pilot : AggregateRoot
    {        
        public string Name { get; set; }

        public int CorporationId { get; set; }

        public int ExternalId { get; set; }

        public DateTime LastUpdate { get; set; }        
    }
}