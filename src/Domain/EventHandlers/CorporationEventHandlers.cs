using System;
using DotNetKillboard.Events;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.EventHandlers
{
    public class CorporationEventHandlers : IEventHandler<CorporationCreated>, IEventHandler<CorporationAllianceChanged>
    {

        private readonly IReportingRepository _repository;

        public CorporationEventHandlers(IReportingRepository repository) {
            _repository = repository;
        }

        public void Handle(CorporationCreated e) {
            _repository.Save(new CorporationDto {
                AllianceId = e.AllianceId,
                ExternalId = e.ExternalId,
                Id = e.Id,
                Name = e.Name,
                Sequence = e.Sequence,
                Timestamp = e.Timestamp
            });
        }

        public void Handle(CorporationAllianceChanged e) {
            var dto = _repository.Get<CorporationDto>(e.Id);
            dto.AllianceId = e.AllianceId;
        }
    }
}