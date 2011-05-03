using DotNetKillboard.Events;
using DotNetKillboard.Reporting;

namespace DotNetKillboard.EventHandlers
{
    public class AllianceEventHandlers : IEventHandler<AllianceCreated>
    {
        private readonly IReportingRepository _repository;

        public AllianceEventHandlers(IReportingRepository repository) {
            _repository = repository;
        }

        public void Handle(AllianceCreated e) {
            var dto = new AllianceDto {
                Id = e.Id,
                Name = e.Name,
                ExternalId = e.ExternalId,
                Timestamp = e.Timestamp
            };

            _repository.Save(dto);
        }
    }
}