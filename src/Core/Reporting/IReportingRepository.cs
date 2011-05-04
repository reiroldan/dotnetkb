using System;
using System.Collections.Generic;
using DotNetKillboard.ReportingQueries;

namespace DotNetKillboard.Reporting
{

    /// <summary>
    /// Reporting repository
    /// </summary>
    public interface IReportingRepository
    {
        /// <summary>
        /// Retrieve a dto by id
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TDto Get<TDto>(Guid id) where TDto : class;

        /// <summary>
        /// Retrieve by example
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="example"></param>
        /// <returns></returns>
        IEnumerable<TDto> GetByExample<TDto>(object example = null) where TDto : class;

        /// <summary>
        /// Save
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="obj"></param>
        void Save<TDto>(TDto obj) where TDto : class;

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="obj"></param>
        void Delete<TDto>(TDto obj) where TDto : class;

        /// <summary>
        /// Initializes a query of type <typeparamref name="TQuery"/>
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <param name="configure">If provided, configures the query</param>
        /// <returns></returns>
        TQuery QueryFor<TQuery>(Action<TQuery> configure = null) where TQuery : class, IQuery;
        
        /// <summary>
        /// Retrieves the next numerical sequence for the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        int GetNextSequenceFor<T>();
    }
}