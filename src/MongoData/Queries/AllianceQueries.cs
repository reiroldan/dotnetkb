using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data.Queries
{
    public class AllianceByNameQuery : MongoQueryBase, IAllianceByNameQuery
    {
        public AllianceDto Execute() {
            return CollectionFor<AllianceDto>().FindOne(Query.EQ("Name", Name));
        }

        public string Name { get; set; }
    }
}