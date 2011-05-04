using System;
using System.Collections.Generic;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingQueries;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data
{
    public class MongoReportingRepository : IReportingRepository
    {
        private readonly IResolver _resolver;
        private readonly string _database;
        private readonly MongoServer _instance;

        public MongoReportingRepository(IResolver resolver, string connectionString, string database) {
            _resolver = resolver;
            _database = database;
            _instance = MongoServer.Create(connectionString);
        }

        public TDto Get<TDto>(Guid id) where TDto : class {
            var collection = GetCollectionFor<TDto>();
            var item = collection.FindOne(Query.EQ("_id", id));
            return item;
        }

        public IEnumerable<TDto> GetByExample<TDto>(object example) where TDto : class {
            throw new NotImplementedException();
        }

        public void Save<TDto>(TDto obj) where TDto : class {
            var collection = GetCollectionFor<TDto>();
            collection.Save(obj);
        }

        public void Delete<TDto>(TDto example) where TDto : class {
            throw new NotImplementedException();
        }

        public TQuery QueryFor<TQuery>(Action<TQuery> configure = null) where TQuery : class, IQuery {
            var filter = _resolver.TryResolve<TQuery>() as TQuery;

            if (filter == null)
                throw new QueryException("Could not resolve a filter for type {0}", typeof(TQuery));

            var mongoFilter = filter as Queries.IMongoQuery;

            if (mongoFilter == null)
                throw new QueryException("Filter for type {0} is not a valid mongo filter", typeof(TQuery));

            mongoFilter.Database = GetDb();

            if (configure != null)
                configure(filter);

            return filter;
        }

        public int GetNextSequenceFor<T>() {
            var collection = GetDb()["NumericSequences"];
            var seqName = CollectionNamesFactory.GetCollectionNameFromType<T>();
            var query = Query.EQ("_id", seqName);
            collection.Update(query, new UpdateBuilder().Inc("next", 1), UpdateFlags.Upsert);
            var result = collection.FindOne(query)["next"];
            return result.AsInt32;            
        }

        #region Internals

        private MongoDatabase GetDb() {
            return _instance[_database];
        }

        private MongoCollection<T> GetCollectionFor<T>() {
            var db = GetDb();
            var name = CollectionNamesFactory.GetCollectionNameFromType<T>();
            return db.GetCollection<T>(name);
        }

        #endregion
    }
}