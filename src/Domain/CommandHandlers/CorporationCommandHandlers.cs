using System;
using DotNetKillboard.Commands;
using DotNetKillboard.Domain;
using DotNetKillboard.Events;

namespace DotNetKillboard.CommandHandlers
{
    public class CorporationCommandHandlers : ICommandHandler<CreateCorporation>, ICommandHandler<ChangeCorporationsAlliance>
    {
        private readonly IDomainRepository _repository;

        public CorporationCommandHandlers(IDomainRepository repository) {
            _repository = repository;
        }

        public void Handle(CreateCorporation command) {
            var item = new Corporation(command.Id, command.Name, command.AllianceId, command.ExternalId);
            _repository.Save(item);
        }

        public void Handle(ChangeCorporationsAlliance command) {
            var item = _repository.GetById<Corporation>(command.Id);
            item.ChangeAlliance(command.AllianceId);
            _repository.Save(item);
        }
    }
}