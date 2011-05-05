using DotNetKillboard.Bus;
using DotNetKillboard.Commands;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;
using DotNetKillboard.ReportingQueries;

namespace DotNetKillboard.Services.Implementation
{
    public class EntitesServiceImpl : IEntitiesService
    {
        private readonly IBus _bus;
        private readonly IReportingRepository _reportingRepository;

        public EntitesServiceImpl(IBus bus, IReportingRepository reportingRepository) {
            _bus = bus;
            _reportingRepository = reportingRepository;
        }

        public AllianceDto GetAlliance(string name) {
            var query = _reportingRepository.QueryFor<IAllianceByNameQuery>(c => c.Name = name);
            var dto = query.Execute();

            if (dto == null) {
                var sequence = _reportingRepository.GetNextSequenceFor<AllianceDto>();
                var uid = SystemIdGenerator.Next();

                _bus.Send(new CreateAlliance(uid, sequence, name, 0));

                dto = new AllianceDto {
                    ExternalId = 0,
                    Id = uid,
                    Name = name,
                    Sequence = sequence,
                    Timestamp = SystemDateTime.Now()
                };
            }

            return dto;
        }

        public CorporationDto GetCorporation(string name, int allianceId) {
            var query = _reportingRepository.QueryFor<ICorporationByNameQuery>(c => c.Name = name);
            var dto = query.Execute();

            if (dto == null) {
                var sequence = _reportingRepository.GetNextSequenceFor<CorporationDto>();
                var uid = SystemIdGenerator.Next();

                _bus.Send(new CreateCorporation(uid, sequence, name, allianceId, 0));

                dto = new CorporationDto {
                    ExternalId = 0,
                    Id = uid,
                    Name = name,
                    Sequence = sequence,
                    AllianceId = allianceId,
                    Timestamp = SystemDateTime.Now()
                };
            } else {
                // if the current alliance is different from what we have in the database
                // update the corp, and all pilots
                if (dto.AllianceId != allianceId) {
                    dto.AllianceId = allianceId;
                    _bus.Send(new ChangeCorporationsAlliance(dto.Id, allianceId));

                    var pilots = _reportingRepository.QueryFor<IPilotsInCorporationQuery>(c => c.Sequence = dto.Sequence).Execute();

                    foreach (var pilot in pilots) {
                        _bus.Send(new ChangePilotsAlliance(pilot, allianceId));
                    }

                }
            }

            return dto;
        }

        public PilotDto GetPilot(string name, int allianceId, int corpId, decimal securityStatus = 0) {
            var query = _reportingRepository.QueryFor<IPilotByNameQuery>(c => c.Name = name);
            var dto = query.Execute();

            if (dto == null) {
                var sequence = _reportingRepository.GetNextSequenceFor<PilotDto>();
                var uid = SystemIdGenerator.Next();

                _bus.Send(new CreatePilot(uid, sequence, name, allianceId, corpId,securityStatus, 0));

                dto = new PilotDto {
                    AllianceId = allianceId,
                    CorporationId = corpId,
                    ExternalId = 0,
                    Id = uid,
                    Name = name,
                    Timestamp = SystemDateTime.Now(),
                    Sequence = sequence
                };
            } else {
                if (dto.CorporationId != corpId) {
                    dto.CorporationId = corpId;
                    _bus.Send(new ChangePilotsCorporation(dto.Id, corpId));
                }
            }

            return dto;
        }
    }
}