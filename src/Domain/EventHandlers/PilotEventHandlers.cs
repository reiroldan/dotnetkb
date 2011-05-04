using DotNetKillboard.Events;
using DotNetKillboard.Reporting;
using DotNetKillboard.ReportingModel;

namespace DotNetKillboard.EventHandlers
{
    public class PilotEventHandlers : IEventHandler<PilotCreated>, IEventHandler<PilotAllianceCorporationChanged>
    {
        private readonly IReportingRepository _repository;

        public PilotEventHandlers(IReportingRepository repository) {
            _repository = repository;
        }

        public void Handle(PilotCreated @event) {
            var dto = new PilotDto {
                Id = @event.Id,
                AllianceId = @event.AllianceId,
                CorporationId = @event.CorporationId,
                ExternalId = @event.ExternalId,
                Name = @event.Name,
                Timestamp = @event.Timestamp,
            };

            _repository.Save(dto);
        }

        public void Handle(PilotAllianceCorporationChanged @event) {
            var dto = _repository.Get<PilotDto>(@event.Id);
            dto.AllianceId = @event.AllianceId;
            dto.CorporationId = @event.CorporationId;
            dto.Timestamp = @event.Timestamp;

            _repository.Save(dto);
        }
    }
}