using System;
using System.Collections.Generic;
using DotNetKillboard.Reporting;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DotNetKillboard.Data
{
    public class MongoReportingRepository : IReportingRepository
    {

        private readonly string _database;
        private readonly MongoServer _instance;

        public MongoReportingRepository(string connectionString, string database) {
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

        #region Internals

        private MongoDatabase GetDb() {
            return _instance[_database];
        }

        private static string GetCollectionNameFromType<T>() {
            return typeof(T).Name;
        }

        private MongoCollection<T> GetCollectionFor<T>() {
            var db = GetDb();
            var name = GetCollectionNameFromType<T>();
            return db.GetCollection<T>(name);
        }

        #endregion
    }
}