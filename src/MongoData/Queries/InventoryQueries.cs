using System.Collections.Generic;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data.Queries
{
    public class SolarSystemByNameQuery : MongoQueryBase, ISolarSystemByNameQuery
    {
        public SolarSystemDto Execute() {
            return CollectionFor<SolarSystemDto>().FindOne(Query.EQ("Name", Name));
        }

        public string Name { get; set; }
    }

    public class ItemByNameQuery : MongoQueryBase, IItemByNameQuery
    {
        public ItemDto Execute() {
            return CollectionFor<ItemDto>().FindOne(Query.EQ("Name", Name));
        }

        public string Name { get; set; }
    }

    public class ItemsWithNamesQuery : MongoQueryBase, IItemsWithNamesQuery
    {
        public IEnumerable<ItemDto> Execute() {
            return CollectionFor<ItemDto>().Find(Query.In("Name", BsonArray.Create(Names)));
        }

        public IEnumerable<string> Names { get; set; }
    }
}