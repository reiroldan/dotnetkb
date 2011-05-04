using System;
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

    
}