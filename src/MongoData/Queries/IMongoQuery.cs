using MongoDB.Driver;

namespace DotNetKillboard.Data.Queries
{
    public interface IMongoQuery
    {
        MongoDatabase Database { get; set; }
    }

    public abstract class MongoQueryBase : IMongoQuery
    {
        public MongoDatabase Database { get; set; }

        protected MongoCollection<T> CollectionFor<T>(string collectionName = null) {
            collectionName = collectionName ?? CollectionNamesFactory.GetCollectionNameFromType<T>();
            return Database.GetCollection<T>(collectionName);
        }
    }
}