using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.ReportingQueries
{
    public interface ICorporationByNameQuery : ISingleQuery<CorporationDto>
    {
        string Name { get; set; }
    }
}