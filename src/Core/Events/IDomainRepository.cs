using System;
using DotNetKillboard.Domain;

namespace DotNetKillboard.Events
{

    /// <summary>
    /// Domain repository
    /// </summary>
    public interface IDomainRepository
    {
        /// <summary>
        /// Persists the aggregate root
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aggregate"></param>
        void Save<T>(T aggregate) where T : AggregateRoot;

        /// <summary>
        /// Retrieves an aggregate root by its identifier
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById<T>(Guid id) where T : AggregateRoot, new();
    }
}