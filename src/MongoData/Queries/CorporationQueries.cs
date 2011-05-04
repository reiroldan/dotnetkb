using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data.Queries
{
    public class CorporationByNameQuery : MongoQueryBase, ICorporationByNameQuery
    {
        public CorporationDto Execute() {
            return CollectionFor<CorporationDto>().FindOne(Query.EQ("Name", Name));
        }

        public string Name { get; set; }
    }
}