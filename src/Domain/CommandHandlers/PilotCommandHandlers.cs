using DotNetKillboard.Commands;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;

namespace DotNetKillboard.CommandHandlers
{

    public class PilotCommandHandlers : ICommandHandler<CreatePilot>, ICommandHandler<ChangePilotsAllianceAndCorporation>
    {
        private readonly IDomainRepository _domainRepository;

        public PilotCommandHandlers(IDomainRepository domainRepository) {
            _domainRepository = domainRepository;
        }

        public void Handle(CreatePilot command) {
            var item = new Pilot(command.Id, command.Name, command.AllianceId, command.CorporationId, command.ExternalId);
            _domainRepository.Save(item);
        }

        public void Handle(ChangePilotsAllianceAndCorporation command) {
            var item = _domainRepository.GetById<Pilot>(command.Id);
            item.ChangeAllianceAndCorporation(command.AllianceId, command.CorporationId);
            _domainRepository.Save(item);
        }
    }
}