using System.Collections.Generic;

namespace DotNetKillboard.ReportingQueries
{
    public interface IListQuery<T> : IQuery
    {
        IEnumerable<T> Execute();
    }
}