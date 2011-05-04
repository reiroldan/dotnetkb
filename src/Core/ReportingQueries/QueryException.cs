using System;

namespace DotNetKillboard.ReportingQueries
{
    public class QueryException : Exception
    {
        public QueryException(string message)
            : base(message) {
        }

        public QueryException(string message, params object[] args)
            : base(string.Format(message, args)) {
        }
    }
}