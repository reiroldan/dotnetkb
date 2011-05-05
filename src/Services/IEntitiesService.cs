using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.Services
{
    public interface IEntitiesService
    {
        AllianceDto GetAlliance(string name);
        CorporationDto GetCorporation(string name, int allianceId);
        PilotDto GetPilot(string name, int allianceId, int corpId, decimal  securityStatus = 0);
    }
}