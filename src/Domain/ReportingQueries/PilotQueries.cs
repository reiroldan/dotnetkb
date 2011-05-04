using System;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.ReportingQueries
{
    public interface IPilotByNameQuery : ISingleQuery<PilotDto>
    {
        string Name { get; set; }
    }

    public interface IPilotsInCorporationQuery : IListQuery<Guid>
    {
        int Sequence { get; set; }
    }
}