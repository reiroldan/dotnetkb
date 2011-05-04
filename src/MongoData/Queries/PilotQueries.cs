using System;
using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data.Queries
{
    public class PilotByNameQuery : MongoQueryBase, IPilotByNameQuery
    {
        public PilotDto Execute() {
            return CollectionFor<PilotDto>().FindOne(Query.EQ("Name", Name));
        }

        public string Name { get; set; }
    }

    public class PilotsInCorporationQuery : MongoQueryBase, IPilotsInCorporationQuery
    {
        public IEnumerable<Guid> Execute() {
            var items = CollectionFor<PilotDto>().Find(Query.EQ("CorporationId", Sequence)).SetFields("_id").ToList();
            return items.Select(i => i.Id);
        }

        public int Sequence { get; set; }
    }

}