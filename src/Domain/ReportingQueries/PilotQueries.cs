using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.ReportingQueries
{
    public interface IPilotByNameQuery : ISingleQuery<PilotDto>
    {
        string Name { get; set; }
    }
}