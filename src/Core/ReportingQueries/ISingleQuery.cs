namespace DotNetKillboard.ReportingQueries
{
    public interface ISingleQuery<T> : IQuery
    {
        T Execute();
    }
}