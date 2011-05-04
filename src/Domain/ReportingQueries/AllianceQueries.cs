using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.ReportingQueries
{
    public interface IAllianceByNameQuery : ISingleQuery<AllianceDto>
    {
        string Name { get; set; }
    }
}