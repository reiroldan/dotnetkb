using System;
using DotNetKillboard.Commands;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;

namespace DotNetKillboard.CommandHandlers
{

    public class PilotCommandHandlers : ICommandHandler<CreatePilot>,
        ICommandHandler<ChangePilotsCorporation>, ICommandHandler<ChangePilotsAlliance>
    {
        private readonly IDomainRepository _domainRepository;

        public PilotCommandHandlers(IDomainRepository domainRepository) {
            _domainRepository = domainRepository;
        }

        public void Handle(CreatePilot command) {
            var item = new Pilot(command.Id, command.Sequence, command.Name, command.AllianceId, command.CorporationId, command.ExternalId);
            _domainRepository.Save(item);
        }

        public void Handle(ChangePilotsCorporation command) {
            var item = _domainRepository.GetById<Pilot>(command.Id);
            item.ChangeCorporation(command.CorporationId);
            _domainRepository.Save(item);
        }

        public void Handle(ChangePilotsAlliance command) {
            var item = _domainRepository.GetById<Pilot>(command.Id);
            item.ChangeAlliance(command.AllianceId);
            _domainRepository.Save(item);
        }
    }
}