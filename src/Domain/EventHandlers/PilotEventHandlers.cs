using DotNetKillboard.Events;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.EventHandlers
{
    public class PilotEventHandlers : IEventHandler<PilotCreated>, IEventHandler<PilotCorporationChanged>
    {
        private readonly IReportingRepository _repository;

        public PilotEventHandlers(IReportingRepository repository) {
            _repository = repository;
        }

        public void Handle(PilotCreated e) {
            var dto = new PilotDto {
                Id = e.Id,
                AllianceId = e.AllianceId,
                CorporationId = e.CorporationId,
                ExternalId = e.ExternalId,
                Name = e.Name,
                Timestamp = e.Timestamp,
            };

            _repository.Save(dto);
        }

        public void Handle(PilotCorporationChanged e) {
            var dto = _repository.Get<PilotDto>(e.Id);
            dto.CorporationId = e.CorporationId;
            dto.Timestamp = e.Timestamp;

            _repository.Save(dto);
        }
    }
}